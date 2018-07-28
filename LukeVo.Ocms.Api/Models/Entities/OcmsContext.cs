using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LukeVo.Ocms.Api.Models.Entities
{
    public partial class OcmsContext : DbContext
    {
        public OcmsContext()
        {
        }

        public OcmsContext(DbContextOptions<OcmsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LedgerAccount> LedgerAccount { get; set; }
        public virtual DbSet<LedgerBook> LedgerBook { get; set; }
        public virtual DbSet<LedgerTransaction> LedgerTransaction { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserClaim> UserClaim { get; set; }
        public virtual DbSet<UserLedgerBook> UserLedgerBook { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LedgerAccount>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.Code })
                    .HasName("IX_LedgerAccount_SearchByCode");

                entity.HasIndex(e => new { e.UserId, e.CreatedTime })
                    .HasName("IX_LedgerAccount_ListForUser");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<LedgerBook>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.CreatedTime })
                    .HasName("IX_LedgerBook_ByUser");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<LedgerTransaction>(entity =>
            {
                entity.HasIndex(e => new { e.CreditAccountId, e.CreatedTime })
                    .HasName("IX_LedgerTransaction_ByCreditAccount");

                entity.HasIndex(e => new { e.DebitAccountId, e.CreatedTime })
                    .HasName("IX_LedgerTransaction_ByDebitAccount");

                entity.HasIndex(e => new { e.BookId, e.Deleted, e.CreatedTime })
                    .HasName("IX_LedgerTransaction_ByBook");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.HasIndex(e => e.Type)
                    .HasName("IX_UserClaim_ByClaimType");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserClaim_ByUser");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserLedgerBook>(entity =>
            {
                entity.HasIndex(e => e.BookId)
                    .HasName("IX_UserLedgerBook_ByBook");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserLedgerBook_ByUser");
            });
        }
    }
}
