using LukeVo.Ocms.Api.Models.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace LukeVo.Ocms.Test.Api.Models.Services
{

    public class AccountServiceTest
    {
        private readonly ITestOutputHelper output;
        private IAccountService accountService = new AccountService();

        public AccountServiceTest(ITestOutputHelper output)
        {
            this.output = output;
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

    }

}
