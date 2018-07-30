using System;
using System.Collections.Generic;

namespace LukeVo.Ocms.Api.Models.Entities
{
    public partial class Log
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public int Level { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
