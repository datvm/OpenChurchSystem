using LukeVo.Ocms.Api.Models.Entities;
using LukeVo.Ocms.Api.Models.Services;
using LukeVo.Ocms.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LukeVo.Ocms.Test.Api.Models.Services
{

    public class AccountServiceTest : IClassFixture<BasicFixture>
    {

        BasicFixture basicFixture;
        ITestOutputHelper output;
        IAccountService accountService;

        public AccountServiceTest(BasicFixture basicFixture, ITestOutputHelper output)
        {
            this.basicFixture = basicFixture;
            this.output = output;

            this.accountService = new AccountService(
                basicFixture.DbContext,
                basicFixture.AppSettings);
        }

        #region Passwords

        [Theory]
        [InlineData("123", "1oeL3uF0DWr4qxVa2kYMDbfXRdbe/qRpxrwm0H5fh5d/OVan")]
        [InlineData("456", "+Fmf9Mek0YoYMeNklQBAFnPgW5fUn/FDErdMD/RnbfGbPQAG")]
        [InlineData("Luke", "B33tuIiCiMM/lXLTqRcBFd2MMYdg4lCljYoLvFRs6jA7AEsm")]
        [InlineData("abc123!@#", "Naw4tT4BZzZjfRXO+UXlSNqFKM+YhhQM9qPT95xrX2bz7iWG")]
        public void ValidatePassword_ShouldMatch(string password, string hash)
        {
            Assert.True(this.accountService.ValidatePassword(password, hash));
        }

        [Theory]
        [InlineData("123", "+Fmf9Mek0YoYMeNklQBAFnPgW5fUn/FDErdMD/RnbfGbPQAG")]
        [InlineData("456", "1oeL3uF0DWr4qxVa2kYMDbfXRdbe/qRpxrwm0H5fh5d/OVan")]
        [InlineData("Luke", "Naw4tT4BZzZjfRXO+UXlSNqFKM+YhhQM9qPT95xrX2bz7iWG")]
        [InlineData("abc123!@#", "B33tuIiCiMM/lXLTqRcBFd2MMYdg4lCljYoLvFRs6jA7AEsm")]
        public void ValidatePassword_ShouldNotMatch(string password, string hash)
        {
            Assert.False(this.accountService.ValidatePassword(password, hash));
        }

        [Theory]
        [InlineData("123")]
        [InlineData("456")]
        [InlineData("Luke")]
        [InlineData("abc123!@#")]
        public void HashPassword_ShouldMatch(string password)
        {
            var result = accountService.HashPassword(password);

            this.output.WriteLine(result);
            
            Assert.True(accountService.ValidatePassword(password, result));
            Assert.False(accountService.ValidatePassword(password + "foo", result));
        }

        #endregion

        #region Admin Account

        [Fact]
        public async Task InitializeAdminAccount_ShouldCreate()
        {
            await this.DeleteAdminAccountIfExist();

            await this.accountService.InitializeAdminAccountAsync(false);

            await this.ValidateAdminAccountAsync(this.basicFixture.AppSettings.InitialAdmin.Password);
        }

        [Fact]
        public async Task InitializeAdminAccount_ShouldIgnore()
        {
            await this.DeleteAdminAccountIfExist();


            await this.accountService.InitializeAdminAccountAsync(false);

            await this.ValidateAdminAccountAsync(this.basicFixture.AppSettings.InitialAdmin.Password);
        }

        private async Task CreateAdmin(string password)
        {
            await this.DeleteAdminAccountIfExist();

            var hashedPassword = this.accountService.HashPassword(password);

            var adminUser = new User()
            {
                Email = this.basicFixture.AppSettings.InitialAdmin.Email,
                PasswordHash = hashedPassword,
            };

            this.basicFixture.DbContext.User.Add(adminUser);

        }

        private async Task DeleteAdminAccountIfExist()
        {
            var adminSettings = this.basicFixture.AppSettings.InitialAdmin;

            var adminUser = await this.basicFixture.DbContext.User
                .FirstOrDefaultAsync(q => q.Email == adminSettings.Email);

            if (adminUser != null)
            {
                this.basicFixture.DbContext.User.Remove(adminUser);
                await this.basicFixture.DbContext.SaveChangesAsync();
            }
        }

        private async Task<User> GetAdminAsync()
        {
            return await this.basicFixture.DbContext.User
                .FirstOrDefaultAsync(q => q.Email == this.basicFixture.AppSettings.InitialAdmin.Email);
        }

        private async Task<User> ValidateAdminAccountAsync(string expectedPassword)
        {
            var adminUser = await this.accountService.GetLoginUserAsync(
                this.basicFixture.AppSettings.InitialAdmin.Email,
                this.basicFixture.AppSettings.InitialAdmin.Password);

            if (adminUser == null)
            {
                return null;
            }

            var adminClaimType = AccountServiceConstants.SysAdminClaimType;
            var adminClaim = await this.basicFixture.DbContext.UserClaim
                .FirstOrDefaultAsync(q => q.UserId == adminUser.Id && q.Type == adminClaimType);

            if (adminClaim == null)
            {
                return null;
            }

            if (expectedPassword != null)
            {
                if (!this.accountService.ValidatePassword(expectedPassword, adminUser.PasswordHash))
                {
                    return null;
                }
            }

            return adminUser;
        }

        #endregion

    }

}
