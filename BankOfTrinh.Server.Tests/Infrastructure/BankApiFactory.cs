using BankOfTrinh.Server.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<BankDbContext>>();
            services.RemoveAll<BankDbContext>();

            services.AddDbContext<BankDbContext>(options =>
            {
                options.UseSqlServer(_connectionString);
            });

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<BankDbContext>();

            dbContext.Database.Migrate();
        });
    }
}