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

        [Theory]
        [InlineData("123")]
        [InlineData("456")]
        [InlineData("Luke")]
        [InlineData("abc123!@#")]
        public void HashPassword_ShouldMatch(string password)
        {
            var result = accountService.HashPassword(password);

            this.output.WriteLine(result);
            this.output.WriteLine(result.Length.ToString());

            Assert.True(accountService.ValidatePassword(password, result));
            Assert.False(accountService.ValidatePassword(password + "foo", result));
        }

        [Fact]
        public async Task InitializeAdminAccount_ShouldCreate()
        {
            await this.DeleteAdminAccountIfExist();

            await this.accountService.InitializeAdminAccount(false);

            await this.ValidateAdminAccountAsync(this.basicFixture.AppSettings.InitialAdmin.Password);
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

    }

}
