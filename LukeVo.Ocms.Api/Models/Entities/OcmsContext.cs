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
        public virtual DbSet<Log> Log { get; set; }
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

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LedgerAccount)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LedgerAccount_User");
            });

            modelBuilder.Entity<LedgerBook>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.CreatedTime })
                    .HasName("IX_LedgerBook_ByUser");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LedgerBook)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LedgerBook_User");
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

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.LedgerTransaction)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LedgerTransaction_LedgerBook");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.LedgerTransaction)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LedgerTransaction_User");

                entity.HasOne(d => d.CreditAccount)
                    .WithMany(p => p.LedgerTransactionCreditAccount)
                    .HasForeignKey(d => d.CreditAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LedgerTransaction_LedgerAccount");

                entity.HasOne(d => d.DebitAccount)
                    .WithMany(p => p.LedgerTransactionDebitAccount)
                    .HasForeignKey(d => d.DebitAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LedgerTransaction_LedgerAccount1");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasIndex(e => new { e.CreatedTime, e.Type })
                    .HasName("IX_Log_TimeAndLevel");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
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

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserClaim)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserClaim_User");
            });

            modelBuilder.Entity<UserLedgerBook>(entity =>
            {
                entity.HasIndex(e => e.BookId)
                    .HasName("IX_UserLedgerBook_ByBook");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_UserLedgerBook_ByUser");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLedgerBook)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserLedgerBook_User");
            });
        }
    }
}
