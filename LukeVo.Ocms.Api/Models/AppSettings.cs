using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LukeVo.Ocms.Api.Models
{
    public class AppSettings
    {

        public InitAdminSettings InitialAdmin { get; set; }
        public JwtSettings Jwt { get; set; }

        public class InitAdminSettings
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ResetToken { get; set; }
        }

        public class JwtSettings
        {
            public string Audience { get; set; }
            public string Issuer { get; set; }
            public string SecurityKey { get; set; }
            public int? TokenLifetime { get; set; }
        }

    }
}
