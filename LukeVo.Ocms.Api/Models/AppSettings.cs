using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LukeVo.Ocms.Api.Models
{
    public class AppSettings
    {

        public InitAdminSettings InitialAdmin { get; set; }

        public class InitAdminSettings
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string RefreshToken { get; set; }
        }

    }
}
