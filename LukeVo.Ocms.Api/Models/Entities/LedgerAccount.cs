using System;
using System.Collections.Generic;

namespace LukeVo.Ocms.Api.Models.Entities
{
    public partial class LedgerAccount
    {
        public LedgerAccount()
        {
            LedgerTransactionCreditAccount = new HashSet<LedgerTransaction>();
            LedgerTransactionDebitAccount = new HashSet<LedgerTransaction>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }

        public User User { get; set; }
        public ICollection<LedgerTransaction> LedgerTransactionCreditAccount { get; set; }
        public ICollection<LedgerTransaction> LedgerTransactionDebitAccount { get; set; }
    }
}
