using BankOfTrinh.Server.Domain.Accounts;
using BankOfTrinh.Server.Domain.Customers;
using BankOfTrinh.Server.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace BankOfTrinh.Server.Data;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions<BankDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();

    public DbSet<AccountTransaction> AccountTransactions => Set<AccountTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BankDbContext).Assembly);
    }
}