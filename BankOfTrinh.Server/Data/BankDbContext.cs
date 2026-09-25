using BankOfTrinh.Server.Domain.Accounts;
using BankOfTrinh.Server.Domain.Customers;
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

}