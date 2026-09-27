# BankOfTrinh.Server

The backend API for BankOfTrinh, a banking simulator built for learning and portfolio practice.

The project is intentionally designed as a modular monolith. It is being used to practice C#, ASP.NET Core, Entity Framework Core, SQL Server, API design, database migrations, validation, integration testing, and automated testing without overengineering the application.

## Current Status

The initial database, domain model, customer management, bank account management, customer-account retrieval, validation, and automated integration test infrastructure are in place.

Implemented features and infrastructure include:

- **Domain and data model**
  - `Customer` and `BankAccount` entities
  - Customer-to-bank-account relationship
  - `BankDbContext` and EF Core entity configurations
  - SQL Server integration with a development connection string stored using .NET User Secrets

- **Database and persistence**
  - EF Core migrations and database creation
  - Unique constraints for customer email addresses and bank account numbers
  - Persistent customer and bank account storage
  - Generated bank account numbers and initial zero balances

- **Customer management**
  - Create and retrieve customers through API endpoints
  - Request validation
  - Duplicate email handling
  - Missing-customer handling

- **Bank account management**
  - Create and retrieve individual bank accounts
  - Retrieve all bank accounts belonging to a customer
  - Associate new bank accounts with existing customers
  - Missing-customer and missing-account handling
  - Initial account balance of zero
  - Generated bank account numbers

- **API and development tooling**
  - Swagger/OpenAPI support
  - RESTful API endpoints for customers and bank accounts
  - Feature-oriented application structure
  - Successful project build

- **Automated testing**
  - Dedicated xUnit test project
  - ASP.NET Core integration testing with `WebApplicationFactory`
  - SQL Server test database provisioned with Testcontainers
  - Test database migrations using the application's EF Core migrations
  - Test configuration that overrides the development connection string
  - Reusable `TestApiClient` for common test operations
  - Tests for customer creation and retrieval
  - Tests for bank account creation and retrieval
  - Tests for retrieving all bank accounts for a customer
  - Tests for validation failures and duplicate customer emails
  - Tests for missing customers and missing bank accounts
  - Tests for database persistence and customer-account associations
  - Tests for unique bank account numbers

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
│   │   │   ├── CreateBankAccount/
│   │   │   │   ├── CreateBankAccountEndpoint.cs
│   │   │   │   ├── CreateBankAccountRequest.cs
│   │   │   │   ├── CreateBankAccountResponse.cs
│   │   │   │   ├── CreateBankAccountService.cs
│   │   │   │   └── CustomerNotFoundException.cs
│   │   │   ├── GetBankAccount/
│   │   │   │   ├── GetBankAccountEndpoint.cs
│   │   │   │   ├── GetBankAccountRequest.cs
│   │   │   │   ├── GetBankAccountResponse.cs
│   │   │   │   ├── GetBankAccountService.cs
│   │   │   │   └── BankAccountNotFoundException.cs
│   │   │   └── GetCustomerBankAccounts/
│   │   │       ├── GetCustomerBankAccountsEndpoint.cs
│   │   │       ├── GetCustomerBankAccountsResponse.cs
│   │   │       └── GetCustomerBankAccountsService.cs
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
    │   ├── CreateBankAccountTests.cs
    │   ├── GetBankAccountTests.cs
    │   └── GetCustomerBankAccountsTests.cs
    ├── Customers/
    │   ├── CreateCustomerTests.cs
    │   └── GetCustomerTests.cs
    └── Infrastructure/
        ├── BankApiFactory.cs
        ├── DatabaseTestCollection.cs
        ├── SqlServerFixture.cs
        └── TestApiClient.cs

The recommended next step is to implement bank account transactions, starting with a deposit operation.