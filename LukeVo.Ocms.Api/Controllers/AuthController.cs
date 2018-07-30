using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Invoke;
using LukeVo.Ocms.Api.Models;
using LukeVo.Ocms.Api.Models.Services;
using LukeVo.Ocms.Api.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LukeVo.Ocms.Api.Controllers
{

    [Authorize, Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        IAccountService accountService;
        IJwtService jwtService;
        AppSettings appSettings;

        public AuthController(IAccountService accountService, IJwtService jwtService, AppSettings appSettings)
        {
            this.accountService = accountService;
            this.jwtService = jwtService;
            this.appSettings = appSettings;
        }

        [AllowAnonymous, HttpGet, Route("initialize-admin")]
        public async Task<IActionResult> InitializeAdmin(string resetToken, bool? force)
        {
            if (resetToken.IsNullOrEmpty() ||
                resetToken != this.appSettings.InitialAdmin.ResetToken)
            {
                return this.BadRequest();
            }

            return await this.InvokeServiceAsync(async () =>
            {
                await this.accountService.InitializeAdminAccountAsync(force == true);
            });
        }

        [AllowAnonymous, HttpGet, Route("token")]
        public async Task<ActionResult<string>> Token([FromQuery] LoginViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            return await this.InvokeServiceAsync(async () =>
            {
                var user = await this.accountService.GetLoginUserAsync(model.Email, model.Password);

                if (user == null)
                {
                    throw new ServiceException("Invalid username or password", System.Net.HttpStatusCode.Unauthorized);
                }

                var claims = await this.accountService.GetUserClaimsAsync(user.Id);
                var token = this.jwtService.IssueToken(claims);

                return token;
            });
        }

        [HttpPost, Route("password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            return await this.InvokeServiceAsync(async () =>
            {

            });
        }

    }

}