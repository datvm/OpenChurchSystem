using System;
using System.Collections.Generic;

namespace LukeVo.Ocms.Api.Models.Entities
{
    public partial class LedgerTransaction
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int CreditAccountId { get; set; }
        public int DebitAccountId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Deleted { get; set; }

        public LedgerBook Book { get; set; }
        public User CreatedByUser { get; set; }
        public LedgerAccount CreditAccount { get; set; }
        public LedgerAccount DebitAccount { get; set; }
    }
}
