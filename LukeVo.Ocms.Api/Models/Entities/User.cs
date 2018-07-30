using System;
using System.Collections.Generic;

namespace LukeVo.Ocms.Api.Models.Entities
{
    public partial class User
    {
        public User()
        {
            LedgerAccount = new HashSet<LedgerAccount>();
            LedgerBook = new HashSet<LedgerBook>();
            LedgerTransaction = new HashSet<LedgerTransaction>();
            UserClaim = new HashSet<UserClaim>();
            UserLedgerBook = new HashSet<UserLedgerBook>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool CanLogin { get; set; }

        public ICollection<LedgerAccount> LedgerAccount { get; set; }
        public ICollection<LedgerBook> LedgerBook { get; set; }
        public ICollection<LedgerTransaction> LedgerTransaction { get; set; }
        public ICollection<UserClaim> UserClaim { get; set; }
        public ICollection<UserLedgerBook> UserLedgerBook { get; set; }
    }
}
