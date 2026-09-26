using BankOfTrinh.Server.Features.Accounts.CreateBankAccount;
using BankOfTrinh.Server.Features.Customers.CreateCustomer;
using BankOfTrinh.Server.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace BankOfTrinh.Server.Tests.Accounts;

[Collection("Database collection")]
public sealed class GetBankAccountTests : IAsyncLifetime
{
    private readonly SqlServerFixture _sqlServerFixture;

    private BankApiFactory _factory = null!;

    private HttpClient _client = null!;

    public GetBankAccountTests(SqlServerFixture sqlServerFixture)
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
    public async Task GetBankAccount_WithExistingAccount_ReturnsAccount()
    {
        // Arrange
        var customer = await CreateCustomerAsync();

        var createdAccount = await CreateBankAccountAsync(customer.Id);

        // Act
        var response = await _client.GetAsync($"/api/accounts/{createdAccount.Id}");

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.True(
            response.IsSuccessStatusCode,
            $"Expected a successful response, but received {(int)response.StatusCode} {response.StatusCode}.\n" +
            $"Response body: {responseBody}");

        // Assert
        //response.EnsureSuccessStatusCode();

        var account = await response.Content
            .ReadFromJsonAsync<CreateBankAccountResponse>();

        Assert.NotNull(account);
        Assert.Equal(createdAccount.Id, account.Id);
        Assert.Equal(customer.Id, account.CustomerId);
        Assert.Equal(createdAccount.AccountNumber, account.AccountNumber);
        Assert.Equal(createdAccount.Balance, account.Balance);
        Assert.Equal(createdAccount.CreatedAtUtc, account.CreatedAtUtc);
    }
}