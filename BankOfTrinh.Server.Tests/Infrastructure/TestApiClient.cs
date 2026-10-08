using BankOfTrinh.Server.Features.Accounts.CreateBankAccount;
using BankOfTrinh.Server.Features.Accounts.GetBankAccount;
using BankOfTrinh.Server.Features.Customers.CreateCustomer;
using BankOfTrinh.Server.Features.Transactions.Deposit;
using System.Net.Http.Json;

namespace BankOfTrinh.Server.Tests.Infrastructure;

public sealed class TestApiClient
{
    private readonly HttpClient _client;

    public TestApiClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<CreateCustomerResponse> CreateCustomerAsync(
        CancellationToken cancellationToken = default)
    {
        var request = new CreateCustomerRequest(
            FirstName: "Test",
            LastName: "Customer",
            Email: $"{Guid.NewGuid()}@example.com");

        var response = await _client.PostAsJsonAsync(
            "/api/customers",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CreateCustomerResponse>(cancellationToken)
            ?? throw new InvalidOperationException("The customer response was empty.");
    }

    public async Task<CreateBankAccountResponse> CreateBankAccountAsync(
    Guid customerId,
    CancellationToken cancellationToken = default)
    {
        var response = await _client.PostAsync(
            $"/api/customers/{customerId}/accounts",
            content: null,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CreateBankAccountResponse>(cancellationToken)
            ?? throw new InvalidOperationException("The account response was empty.");
    }

    public async Task<GetBankAccountResponse> GetBankAccountAsync(
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync(
            $"/api/accounts/{accountId}",
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<GetBankAccountResponse>(cancellationToken)
            ?? throw new InvalidOperationException(
                "The bank account response was empty.");
    }

    public async Task<DepositResponse> DepositAsync(
        string AccountNumber,
        decimal DepositedAmount,
        CancellationToken cancellationToken = default)
    {
        var request = new DepositRequest(DepositedAmount);

        var response = await _client.PostAsJsonAsync(
            $"/api/accounts/{AccountNumber}/deposits",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<DepositResponse>(cancellationToken)
            ?? throw new InvalidOperationException(
                "The deposit response was empty.");
    }

    public async Task<HttpResponseMessage> DepositRawAsync(
        string accountNumber,
        decimal depositedAmount,
        CancellationToken cancellationToken = default)
    {
        var request = new DepositRequest(depositedAmount);

        return await _client.PostAsJsonAsync(
            $"/api/accounts/{accountNumber}/deposits",
            request,
            cancellationToken);
    }
}