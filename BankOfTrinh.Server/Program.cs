using BankOfTrinh.Server.Data;
using BankOfTrinh.Server.Features.Accounts.CreateBankAccount;
using BankOfTrinh.Server.Features.Customers.CreateCustomer;
using BankOfTrinh.Server.Features.Customers.GetCustomer;
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add endpoint services
builder.Services.AddScoped<GetCustomerService>();
builder.Services.AddScoped<CreateCustomerService>();
builder.Services.AddScoped<CreateBankAccountService>();

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

// Register endpoints
app.MapCreateCustomerEndpoint();
app.MapGetCustomerEndpoint();
app.MapCreateBankAccountEndpoint();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

public partial class Program
{
}