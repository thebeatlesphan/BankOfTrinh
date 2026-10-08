using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BankOfTrinh.Ledger;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the ledger. Call from Program.cs:
    /// <code>builder.Services.AddLedger(builder.Configuration.GetConnectionString("BankOfTrinh")!);</code>
    /// </summary>
    public static IServiceCollection AddLedger(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<LedgerDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
                // Separate history table and schema so the ledger's migrations never collide with
                // the existing DbContext's migrations in the same database.
                sql.MigrationsHistoryTable(LedgerDbContext.MigrationsHistoryTable, LedgerDbContext.Schema)));

        services.TryAddSingleton(TimeProvider.System);
        services.AddScoped<IPostingService, PostingService>();
        services.AddScoped<IAccountService, AccountService>();
        return services;
    }
}
