using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LukeVo.Ocms.Api.Models
{

    public static class Extensions
    {

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

    }

}
