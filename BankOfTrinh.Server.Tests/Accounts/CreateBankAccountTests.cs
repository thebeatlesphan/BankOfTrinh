using BankOfTrinh.Server.Data;
using BankOfTrinh.Server.Features.Accounts.CreateBankAccount;
using BankOfTrinh.Server.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
namespace BankOfTrinh.Server.Tests.Accounts;

[Collection("Database collection")]
public sealed class CreateBankAccountTests : IAsyncLifetime
{
    private readonly SqlServerFixture _sqlServerFixture;

    private BankApiFactory _factory = null!;

    private HttpClient _client = null!;

    private TestApiClient _apiClient = null!;

    public CreateBankAccountTests(SqlServerFixture sqlServerFixture)
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
    public async Task CreateBankAccount_WithExistingCustomer_ReturnsCreatedAccount()
    {
        var customer = await _apiClient.CreateCustomerAsync();

        var response = await _client.PostAsync(
            $"/api/customers/{customer.Id}/accounts",
            content: null);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var account = await response.Content.ReadFromJsonAsync<CreateBankAccountResponse>();

        account.Should().NotBeNull();
        account!.CustomerId.Should().Be(customer.Id);
        account.AccountNumber.Should().NotBeNullOrWhiteSpace();
        account.Balance.Should().Be(0m);
    }

    [Fact]
    public async Task CreateBankAccount_WithMissingCustomer_ReturnsNotFound()
    {
        var response = await _client.PostAsync(
            $"/api/customers/{Guid.NewGuid()}/accounts",
            content: null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateBankAccount_PersistsAccountWithCustomerId()
    {
        var customer = await _apiClient.CreateCustomerAsync();

        var response = await _client.PostAsync(
            $"/api/customers/{customer.Id}/accounts",
            content: null);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<CreateBankAccountResponse>();

        created.Should().NotBeNull();

        await using var scope = _factory.Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<BankDbContext>();

        var account = await dbContext.BankAccounts
            .SingleAsync(account => account.Id == created!.Id);

        account.CustomerId.Should().Be(customer.Id);
    }

    [Fact]
    public async Task CreateMultipleBankAccounts_GeneratesUniqueAccountsNumbers()
    {
        var customer = await _apiClient.CreateCustomerAsync();

        var first = await _apiClient.CreateBankAccountAsync(customer.Id);
        var second = await _apiClient.CreateBankAccountAsync(customer.Id);

        first.AccountNumber.Should().NotBe(second.AccountNumber);

    }
}