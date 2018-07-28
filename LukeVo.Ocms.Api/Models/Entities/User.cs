using System;
using System.Collections.Generic;

namespace LukeVo.Ocms.Api.Models.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool CanLogin { get; set; }
    }
}
