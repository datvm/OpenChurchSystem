using System;
using System.Collections.Generic;

namespace LukeVo.Ocms.Api.Models.Entities
{
    public partial class LedgerBook
    {
        public LedgerBook()
        {
            LedgerTransaction = new HashSet<LedgerTransaction>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }

        public User User { get; set; }
        public ICollection<LedgerTransaction> LedgerTransaction { get; set; }
    }
}
