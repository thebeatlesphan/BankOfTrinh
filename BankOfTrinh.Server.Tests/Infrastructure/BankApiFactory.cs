using BankOfTrinh.Server.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BankOfTrinh.Server.Tests.Infrastructure;

public sealed class BankApiFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public BankApiFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        // Make the value available while Program.cs is being built
        builder.UseSetting(
            "ConnectionStrings:BankOfTrinh",
            _connectionString);

        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            var testConnectionString = new Dictionary<string, string?>
            {
                ["ConnectionStrings:BankOfTrinh"] = _connectionString
            };

            configuration.AddInMemoryCollection(testConnectionString);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<BankDbContext>>();
            services.RemoveAll<BankDbContext>();

            services.AddDbContext<BankDbContext>(options =>
            {
                options.UseSqlServer(_connectionString);
            });
        });
    }

    public async Task ApplyMigrationsAsync()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<BankDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}