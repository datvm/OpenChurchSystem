using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LukeVo.Ocms.Api.Models.ViewModels
{

    public class LoginViewModel
    {

        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class ChangePasswordViewModel
    {

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }

}
