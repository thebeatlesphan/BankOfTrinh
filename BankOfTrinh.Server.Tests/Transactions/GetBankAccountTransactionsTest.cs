using BankOfTrinh.Server.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BankOfTrinh.Server.Tests.Transactions;

[Collection("Database collection")]
public sealed class GetBankAccountTransactionsTest : IAsyncLifetime
{
    private readonly SqlServerFixture _sqlServerFixture;

    private BankApiFactory _factory = null!;

    private HttpClient _client = null!;

    private TestApiClient _apiClient = null!;

    public GetBankAccountTransactionsTest(SqlServerFixture sqlServerFixture)
    {
        _sqlServerFixture = sqlServerFixture;
    }

    public async Task InitializeAsync()
    {
        _factory = new BankApiFactory(_sqlServerFixture.ConnectionString);

        await _factory.ApplyMigrationsAsync();

        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        _apiClient = new TestApiClient(_client);
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
    }

    [Fact]

}