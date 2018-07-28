using System;
using System.Collections.Generic;

namespace LukeVo.Ocms.Api.Models.Entities
{
    public partial class UserClaim
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
