using BankOfTrinh.Server.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankOfTrinh.Server.Data;

public sealed class AccountTransactionConfigurations : IEntityTypeConfiguration<AccountTransaction>
{
    public void Configure(EntityTypeBuilder<AccountTransaction> builder)
    {
        builder.ToTable("AccountTransactions");

        builder.HasKey(transaction => transaction.Id);

        builder.Property(transaction => transaction.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(transaction => transaction.BalanceAfterTransaction)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(transaction => transaction.Type)
            .IsRequired();

        builder.Property(transaction => transaction.CreatedAt)
            .IsRequired();

        builder.HasOne(transaction => transaction.BankAccount)
            .WithMany(account => account.Transactions)
            .HasForeignKey(transaction => transaction.BankAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(transaction => new
        {
            transaction.BankAccountId,
            transaction.CreatedAt
        });
    }
}