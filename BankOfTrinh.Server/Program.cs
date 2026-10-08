using BankOfTrinh.Ledger;
using BankOfTrinh.Server.Data;
using BankOfTrinh.Server.Features.Accounts.CreateBankAccount;
using BankOfTrinh.Server.Features.Accounts.Deposit;
using BankOfTrinh.Server.Features.Accounts.GetBankAccount;
using BankOfTrinh.Server.Features.Accounts.GetCustomerBankAccounts;
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
builder.Services.AddScoped<GetBankAccountService>();
builder.Services.AddScoped<GetCustomerBankAccountsService>();
builder.Services.AddScoped<DepositService>();

// Ledger (double-entry money movement)
builder.Services.AddLedger(builder.Configuration.GetConnectionString("BankOfTrinh")!);

// Handle development CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy
            .WithOrigins("https://localhost:56593", "https://127.0.0.1:56593")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AngularClient");

app.UseAuthorization();

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register endpoints
app.MapCreateCustomerEndpoint();
app.MapGetCustomerEndpoint();
app.MapCreateBankAccountEndpoint();
app.MapGetBankAccountEndpoint();
app.MapGetCustomerBankAccountsEndpoint();
app.MapDepositEndpoint();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();

public partial class Program
{
}