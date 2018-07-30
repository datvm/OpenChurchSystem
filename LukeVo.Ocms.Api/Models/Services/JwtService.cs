using JwtSharp;
using LukeVo.Ocms.Api.Models.Entities;
using ServiceSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LukeVo.Ocms.Api.Models.Services
{

    public interface IJwtService : IService
    {

        string IssueToken(IEnumerable<UserClaim> claims);
        ClaimsPrincipal GetPrincipal(string token, bool throwIfNotAuthenticated);

    }

    public class JwtService : IJwtService, IService<IJwtService>
    {

        JwtIssuer jwtIssuer;

        public JwtService(JwtIssuer jwtIssuer)
        {
            this.jwtIssuer = jwtIssuer;
        }

        public string IssueToken(IEnumerable<UserClaim> claims)
        {
            return this.jwtIssuer.IssueToken(claims
                .Select(q => new Claim(q.Type, q.Value)));
        }

        public ClaimsPrincipal GetPrincipal(string token, bool throwIfNotAuthenticated)
        {
            var principal = this.jwtIssuer.GetPrincipal(token);

            if (throwIfNotAuthenticated && !principal.Identity.IsAuthenticated)
            {
                throw new ArgumentException("Unauthorized", nameof(token));
            }

            return principal;
        }

    }

}
