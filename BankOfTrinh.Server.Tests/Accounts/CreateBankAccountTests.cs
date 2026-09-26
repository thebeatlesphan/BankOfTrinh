using BankOfTrinh.Server.Data;
using BankOfTrinh.Server.Features.Accounts.CreateBankAccount;
using BankOfTrinh.Server.Features.Customers.CreateCustomer;
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
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
    }

    public async Task<CreateCustomerResponse> CreateCustomerAsync()
    {
        var request = new CreateCustomerRequest(
            FirstName: "Test",
            LastName: "Customer",
            Email: $"{Guid.NewGuid()}@example.com");

        var response = await _client.PostAsJsonAsync(
            "/api/customers",
            request);

        response.EnsureSuccessStatusCode();

        var customer = await response.Content.ReadFromJsonAsync<CreateCustomerResponse>();

        return customer
            ?? throw new InvalidOperationException("The custome response was empty.");
    }

    public async Task<CreateBankAccountResponse> CreateBankAccountAsync(
        Guid customerId)
    {
        var response = await _client.PostAsync(
            $"/api/customers/{customerId}/accounts",
            content: null);

        response.EnsureSuccessStatusCode();

        var account = await response.Content.ReadFromJsonAsync<CreateBankAccountResponse>();

        return account
            ?? throw new InvalidOperationException(
                "The account response was empty.");
    }

    [Fact]
    public async Task CreateBankAccount_WithExistingCustomer_ReturnsCreatedAccount()
    {
        var customer = await CreateCustomerAsync();

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
        var customer = await CreateCustomerAsync();

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
        var customer = await CreateCustomerAsync();

        var first = await CreateBankAccountAsync(customer.Id);
        var second = await CreateBankAccountAsync(customer.Id);

        first.AccountNumber.Should().NotBe(second.AccountNumber);

    }
}