using ServiceSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LukeVo.Ocms.Api.Models.Services
{

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

    }

    public class AccountService : IAccountService, IService<IAccountService>
    {
        private const int HashIteration = 10000;

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

    }

}
