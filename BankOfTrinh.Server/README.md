# BankOfTrinh.Server

The backend API for BankOfTrinh, a banking simulator built for learning and portfolio practice.

The project is intentionally designed as a modular monolith. It is being used to practice C#, ASP.NET Core, Entity Framework Core, SQL Server, API design, database migrations, validation, integration testing, and automated testing without overengineering the application.

## Current State

The initial database, domain model, customer creation and retrieval features, validation, and automated integration test infrastructure are in place.

Implemented:

- `Customer` domain entity
- `BankAccount` domain entity
- `BankDbContext`
- SQL Server configuration
- Development connection string using .NET User Secrets
- Entity Framework Core entity configurations
- Initial database migration
- SQL Server database creation through EF Core migrations
- Unique customer email constraint
- Customer creation feature
- Customer creation API endpoint
- Customer retrieval feature
- Customer retrieval API endpoint
- Request validation
- Duplicate customer email handling
- Not-found handling for missing customers
- Swagger/OpenAPI support
- Successful project build
- Dedicated xUnit test project
- ASP.NET Core integration testing with `WebApplicationFactory`
- SQL Server test database using Testcontainers
- Test database migrations using the application’s EF Core migrations
- Test configuration overriding the development connection string
- Tests for successful customer creation
- Tests for validation failures
- Tests for duplicate customer emails
- Tests for database persistence
- Tests for retrieving an existing customer
- Tests for retrieving a missing customer

## Project Structure

```text
BankOfTrinh/
├── bankoftrinh.client/       # Angular frontend
├── BankOfTrinh.Server/       # ASP.NET Core backend API
│   ├── Data/
│   │   ├── BankAccountConfigurations.cs
│   │   ├── CustomerConfigurations.cs
│   │   ├── Migrations/
│   │   └── BankDbContext.cs
│   ├── Domain/
│   │   ├── Accounts/
│   │   │   └── BankAccount.cs
│   │   └── Customers/
│   │       └── Customer.cs
│   ├── Features/
│   │   ├── Accounts/
│   │   │   └── CreateBankAccount/
│   │   │       ├── CreateBankAccountEndpoint.cs
│   │   │       ├── CreateBankAccountRequest.cs
│   │   │       ├── CreateBankAccountResponse.cs
│   │   │       ├── CreateBankAccountService.cs
│   │   │       └── CustomerNotFoundException.cs
│   │   └── Customers/
│   │       ├── CreateCustomer/
│   │       │   ├── CreateCustomerEndpoint.cs
│   │       │   ├── CreateCustomerRequest.cs
│   │       │   ├── CreateCustomerResponse.cs
│   │       │   ├── CreateCustomerService.cs
│   │       │   └── CustomerEmailAlreadyExistsException.cs
│   │       └── GetCustomer/
│   │           ├── GetCustomerEndpoint.cs
│   │           ├── GetCustomerRequest.cs
│   │           ├── GetCustomerResponse.cs
│   │           └── GetCustomerService.cs
│   ├── Program.cs
│   └── appsettings.json
└── BankOfTrinh.Server.Tests/
    ├── Accounts/
    │   └── CreateBankAccountTests.cs
    ├── Customers/
    │   ├── CreateCustomerTests.cs
    │   └── GetCustomerTests.cs
    └── Infrastructure/
        ├── BankApiFactory.cs
        ├── DatabaseTestCollection.cs
        └── SqlServerFixture.cs