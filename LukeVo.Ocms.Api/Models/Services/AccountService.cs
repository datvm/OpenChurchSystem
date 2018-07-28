using LukeVo.Ocms.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LukeVo.Ocms.Api.Models.Services
{

    public static class AccountServiceConstants
    {

        public const string SysAdminClaimType = "SysAdmin";

    }

    public interface IAccountService : IService
    {

        /// <summary>
        /// Hash a Password and returns a pair of Hashed/Salt.
        /// </summary>
        string HashPassword(string rawPassword);

        /// <summary>
        /// Check if a password is correct
        /// </summary>
        bool ValidatePassword(string enteredPassword, string hashed);

        /// <summary>
        /// Validate the Login credential
        /// </summary>
        Task<User> GetLoginUserAsync(string email, string enteredPassword);

        Task InitializeAdminAccount(bool force);

    }

    public class AccountService : BaseService, IAccountService, IService<IAccountService>
    {
        const int HashIteration = 10000;
        
        AppSettings appSettings;

        public AccountService(OcmsContext dbContext, AppSettings appSettings) : base(dbContext)
        {
            this.appSettings = appSettings;
        }

        public string HashPassword(string rawPassword)
        {
            var salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(rawPassword, salt, HashIteration);
            var hash = pbkdf2.GetBytes(20);

            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }

        public bool ValidatePassword(string enteredPassword, string hashed)
        {
            if (hashed.IsNullOrEmpty())
            {
                return false;
            }

            var hashBytes = Convert.FromBase64String(hashed);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, HashIteration);

            var enteredPasswordHash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != enteredPasswordHash[i])
                {
                    return false;
                }

            }

            return true;
        }

        public async Task InitializeAdminAccount(bool force)
        {
            var adminEmail = this.appSettings.InitialAdmin.Email;
            var adminUser = await this.GetUserByEmailAsync(adminEmail);

            // Only create/update if there is no admin account,
            // or Force to update the password
            if (adminUser == null || force)
            {
                if (adminUser == null)
                {
                    adminUser = new User()
                    {
                        Email = adminEmail,
                        Name = adminEmail,
                    };

                    this.DbContext.Add(adminUser);
                }

                var passwordHash = this.HashPassword(this.appSettings.InitialAdmin.Password);
                adminUser.PasswordHash = passwordHash;

                await this.DbContext.SaveChangesAsync();
            }

            // Set Claim if not
            var adminClaim = await this.DbContext.UserClaim
                .FirstOrDefaultAsync(q => q.UserId == adminUser.Id && q.Type == AccountServiceConstants.SysAdminClaimType);

            if (adminClaim == null)
            {
                adminClaim = new UserClaim()
                {
                    UserId = adminUser.Id,
                    Type = AccountServiceConstants.SysAdminClaimType,
                    Value = "1",
                };
                this.DbContext.UserClaim.Add(adminClaim);

                await this.DbContext.SaveChangesAsync();
            }
        }

        public async Task<User> GetLoginUserAsync(string email, string enteredPassword)
        {
            var user = await this.GetUserByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            if (this.ValidatePassword(enteredPassword, user.PasswordHash))
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        private async Task<User> GetUserByEmailAsync(string email)
        {
            return await this.DbContext.User
                .FirstOrDefaultAsync(q => q.Email == email);
        }

    }

}
