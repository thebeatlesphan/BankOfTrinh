using BankOfTrinh.Server.Data;
using BankOfTrinh.Server.Features.Customers.CreateCustomer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("BankOfTrinh")
?? throw new InvalidOperationException(
    "Connection string 'BankOfTrinh' was not found.");

builder.Services.AddDbContext<BankDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CreateCustomerService>();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCreateCustomerEndpoint();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

// Debugging endpoints
var endpointsDataSources = app.Services
    .GetServices<EndpointDataSource>();

foreach (var dataSource in endpointsDataSources)
{
    foreach (var endpoint in dataSource.Endpoints)
    {
        Console.WriteLine(
            $"{endpoint.DisplayName} - " +
            $"{string.Join(", ", endpoint.Metadata.OfType<HttpMethodMetadata>()
            .SelectMany(m => m.HttpMethods))}");
    }
}

app.Run();

public partial class Program
{
}