using BankOfTrinh.Server.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace BankOfTrinh.Server.Tests.Customers;

[Collection("Database collection")]
public sealed class GetCustomerTests : IAsyncLifetime
{
    private readonly SqlServerFixture _sqlServerFixture;
    private BankApiFactory _factory = null!;
    private HttpClient _client = null!;

    public GetCustomerTests(SqlServerFixture sqlServerFixture)
    {
        _sqlServerFixture = sqlServerFixture;
    }

    public async Task InitializeAsync()
    {
        _factory = new BankApiFactory(_sqlServerFixture.ConnectionString);

        await _factory.ApplyMigrationsAsync();

        _client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task Get_existing_customer_returns_customer()
    {
        var email = $"get-{Guid.NewGuid():N}@example.com";

        var createRequest = new
        {
            firstName = "Trinh",
            lastName = "Dong",
            email
        };

        var createResponse = await _client.PostAsJsonAsync("/api/customers", createRequest);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdCustomer = await createResponse.Content
            .ReadFromJsonAsync<CreateCustomerResponse>();

        createdCustomer.Should().NotBeNull();

        var response = await _client.GetAsync($"/api/customers/{createdCustomer!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var customer = await response.Content
            .ReadFromJsonAsync<GetCustomerResponse>();

        customer.Should().NotBeNull();
        customer!.Id.Should().Be(createdCustomer.Id);
        customer.FirstName.Should().Be("Trinh");
        customer.LastName.Should().Be("Dong");
        customer.Email.Should().Be(email);
    }

    [Fact]
    public async Task Get_missing_customer_returns_not_found()
    {
        var response = await _client.GetAsync(
            $"/api/customers/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    private sealed record CreateCustomerResponse(
    Guid Id,
    string Email);

    private sealed record GetCustomerResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email);

}