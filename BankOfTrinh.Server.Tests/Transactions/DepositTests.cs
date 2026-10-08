using BankOfTrinh.Server.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace BankOfTrinh.Server.Tests.Transactions;

[Collection("Database collection")]
public sealed class DepositTests : IAsyncLifetime
{
    private readonly SqlServerFixture _sqlServerFixture;

    private BankApiFactory _factory = null!;

    private HttpClient _client = null!;

    private TestApiClient _apiClient = null!;

    public DepositTests(SqlServerFixture sqlServerFixture)
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
    public async Task Deposit_WithValidAmount_IncreasesAccountBalance()
    {
        var customer = await _apiClient.CreateCustomerAsync();

        var account = await _apiClient.CreateBankAccountAsync(customer.Id);

        const decimal depositAmount = 100.00m;

        var deposit = await _apiClient.DepositAsync(
            account.AccountNumber,
            depositAmount);

        Assert.Equal(account.AccountNumber, deposit.AccountNumber);
        Assert.Equal(depositAmount, deposit.DepositedAmount);
        Assert.Equal(depositAmount, deposit.NewBalance);

        var updatedAccount = await _apiClient.GetBankAccountAsync(account.Id);

        Assert.Equal(depositAmount, updatedAccount.Balance);
    }

    [Fact]
    public async Task Deposit_Twice_AccumulatesTheAccountBalance()
    {
        var customer = await _apiClient.CreateCustomerAsync();

        var account = await _apiClient.CreateBankAccountAsync(customer.Id);

        var firstDeposit = await _apiClient.DepositAsync(
            account.AccountNumber,
            100.00m);

        var secondDeposit = await _apiClient.DepositAsync(
            account.AccountNumber,
            50.00m);

        Assert.Equal(100.00m, firstDeposit.NewBalance);
        Assert.Equal(150.00m, secondDeposit.NewBalance);

        var updatedAccount = await _apiClient.GetBankAccountAsync(account.Id);

        Assert.Equal(150.0m, updatedAccount.Balance);
    }

    [Fact]
    public async Task Deposit_CreatesUniqueTransactionIdentifiers()
    {
        var customer = await _apiClient.CreateCustomerAsync();

        var account = await _apiClient.CreateBankAccountAsync(customer.Id);

        var firstDeposit = await _apiClient.DepositAsync(
            account.AccountNumber,
            25.00m);

        var secondDeposit = await _apiClient.DepositAsync(
            account.AccountNumber,
            75.00m);

        Assert.NotEqual(
            firstDeposit.TransactionId,
            secondDeposit.TransactionId);
    }

    [Fact]
    public async Task Deposit_WithNegativeAmount_ReturnsBadRequest()
    {
        var customer = await _apiClient.CreateCustomerAsync();

        var account = await _apiClient.CreateBankAccountAsync(customer.Id);

        var response = await _apiClient.DepositRawAsync(
            account.AccountNumber,
            -5.00m);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var updatedAccount = await _apiClient.GetBankAccountAsync(account.Id);

        Assert.Equal(0m, updatedAccount.Balance);
    }

    [Fact]
    public async Task Deposit_WithMissingAccount_ReturnsNotFound()
    {
        var missingAccountNumber = "9999999999";

        using var response = await _apiClient.DepositRawAsync(
            missingAccountNumber,
            100.00m);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}