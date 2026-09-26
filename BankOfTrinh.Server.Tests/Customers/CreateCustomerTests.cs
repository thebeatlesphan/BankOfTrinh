using BankOfTrinh.Server.Data;
using BankOfTrinh.Server.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace BankOfTrinh.Server.Tests.Customers;

[Collection("Database collection")]
public sealed class CreateCustomerTests : IAsyncLifetime
{
    private readonly SqlServerFixture _sqlServerFixture;
    private BankApiFactory _factory = null!;
    private HttpClient _client = null!;

    public CreateCustomerTests(SqlServerFixture sqlServerFixture)
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

    [Fact]
    public async Task Post_valid_customer_creates_customer()
    {
        var request = new
        {
            firstName = "Trinh",
            lastName = "Dong",
            email = $"trinh-{Guid.NewGuid():N}@examples.com"
        };

        var response = await _client.PostAsJsonAsync("/api/customers", request);

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Location: {response.Headers.Location}");
        Console.WriteLine(await response.Content.ReadAsStringAsync());

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CreateCustomerResponse>();

        result.Should().NotBeNull();
        result!.Email.Should().Be(request.email);
    }

    [Fact]
    public async Task Post_invalid_customer_returns_validation_error()
    {
        var request = new
        {
            firstName = "",
            lastName = "",
            email = "not-an-email"
        };

        var response = await _client.PostAsJsonAsync("/api/customers", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_duplicate_email_returns_conflict()
    {
        var email = $"duplicate-{Guid.NewGuid():N}@example.com";

        var request = new
        {
            firstName = "First",
            lastName = "Customer",
            email
        };

        var firstResponse = await _client.PostAsJsonAsync("/api/customers", request);

        firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var duplicateRequest = new
        {
            firstName = "Second",
            lastName = "Customer",
            email
        };

        var secondResponse = await _client.PostAsJsonAsync("/api/customers", duplicateRequest);

        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Post_valid_customer_persists_customer_to_database()
    {
        var email = $"persisted-{Guid.NewGuid():N}@example.com";

        var request = new
        {
            firstName = "Persisted",
            lastName = "Customer",
            email
        };

        var response = await _client.PostAsJsonAsync("/api/customers", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<BankDbContext>();

        var customer = await dbContext.Customers
            .SingleOrDefaultAsync(x => x.Email == email);

        customer.Should().NotBeNull();
        customer!.Email.Should().Be(email);
        customer.FirstName.Should().Be("Persisted");
        customer.LastName.Should().Be("Customer");
    }

    private sealed record CreateCustomerResponse(
        Guid Id,
        string Email);
}