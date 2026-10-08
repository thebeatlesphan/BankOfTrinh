using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Ledger;

public class LedgerDbContext(DbContextOptions<LedgerDbContext> options) : DbContext(options)
{
    public const string Schema = "ledger";
    public const string MigrationsHistoryTable = "__EFMigrationsHistory_Ledger";

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<LedgerTransaction> Transactions => Set<LedgerTransaction>();
    public DbSet<LedgerEntry> Entries => Set<LedgerEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.Entity<Account>(b =>
        {
            b.ToTable("Accounts");
            b.HasKey(a => a.Id);
            b.Property(a => a.Code).IsRequired().HasMaxLength(100);
            b.HasIndex(a => a.Code).IsUnique();
            b.Property(a => a.Name).IsRequired().HasMaxLength(200);
            b.Property(a => a.Type).HasConversion<string>().HasMaxLength(20);
            b.Property(a => a.OwnerType).HasMaxLength(50);
            b.Property(a => a.OwnerId).HasMaxLength(100);
            b.HasIndex(a => new { a.OwnerType, a.OwnerId });
            b.Property(a => a.Currency).IsRequired().HasMaxLength(3).IsFixedLength();
        });

        modelBuilder.Entity<LedgerTransaction>(b =>
        {
            b.ToTable("LedgerTransactions");
            b.HasKey(t => t.Id);
            b.Property(t => t.IdempotencyKey).IsRequired().HasMaxLength(200);
            b.HasIndex(t => t.IdempotencyKey).IsUnique();
            b.Property(t => t.Description).IsRequired().HasMaxLength(500);
            b.Property(t => t.SourceType).IsRequired().HasMaxLength(100);
            b.Property(t => t.SourceId).IsRequired().HasMaxLength(200);
            b.HasIndex(t => new { t.SourceType, t.SourceId });

            // A transaction can be reversed at most once (nulls are allowed to repeat).
            b.HasIndex(t => t.ReversesTransactionId).IsUnique();
            b.HasOne<LedgerTransaction>()
                .WithMany()
                .HasForeignKey(t => t.ReversesTransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(t => t.Entries)
                .WithOne(e => e.Transaction!)
                .HasForeignKey(e => e.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LedgerEntry>(b =>
        {
            b.ToTable("LedgerEntries", t =>
                t.HasCheckConstraint("CK_LedgerEntries_AmountPositive", "[AmountCents] > 0"));
            b.HasKey(e => e.Id);
            b.Property(e => e.Direction).HasConversion<string>().HasMaxLength(6);
            b.HasIndex(e => e.AccountId);
            b.HasIndex(e => e.TransactionId);
            b.HasOne(e => e.Account!)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    // The ledger is append-only. Refuse to update or delete transactions and entries at the
    // application level. (A database trigger or restricted DB permissions can enforce this too.)
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        EnsureAppendOnly();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        EnsureAppendOnly();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void EnsureAppendOnly()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is LedgerTransaction or LedgerEntry
                && entry.State is EntityState.Modified or EntityState.Deleted)
            {
                throw new InvalidOperationException(
                    $"{entry.Metadata.ClrType.Name} rows are immutable. Post a reversing transaction instead.");
            }
        }
    }
}
