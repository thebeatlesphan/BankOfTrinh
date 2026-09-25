using BankOfTrinh.Server.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankOfTrinh.Server.Data.Configurations;

public sealed class BankAccountConfigurations
    : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.Property(account => account.Balance).HasPrecision(19, 4);
    }
}