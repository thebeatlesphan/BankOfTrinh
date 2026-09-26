using BankOfTrinh.Server.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankOfTrinh.Server.Data.Configurations;

public sealed class BankAccountConfigurations
    : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.HasKey(account => account.Id);

        builder.Property(account => account.AccountNumber)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(account => account.AccountNumber)
            .IsUnique();

        builder.Property(account => account.Balance)
            .HasPrecision(19, 4)
            .IsRequired();

        builder.Property(account => account.CreatedAtUtc)
            .IsRequired();

        builder.HasOne(account => account.Customer)
            .WithMany(customer => customer.BankAccounts)
            .HasForeignKey(account => account.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(account => account.CustomerId);
    }
}