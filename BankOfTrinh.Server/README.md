# BankOfTrinh.Server

The backend API for BankOfTrinh, a banking simulator built for learning and portfolio practice.

The project is intentionally designed as a modular monolith. It is being used to practice C#, ASP.NET Core, Entity Framework Core, SQL Server, API design, database migrations, validation, integration testing, and automated testing without overengineering the application.

## Current Status

The initial database, domain model, customer management, bank account management, customer-account retrieval, account transaction model, validation, and automated integration test infrastructure are in place.

Implemented features and infrastructure include:

- **Domain and data model**
  - `Customer` entity
  - `BankAccount` entity
  - `AccountTransaction` entity
  - `TransactionType` enum
  - Customer-to-bank-account relationship
  - Bank-account-to-transaction relationship
  - Account balance management through domain methods
  - Transaction balance snapshots using `BalanceAfterTransaction`
  - `BankDbContext` and EF Core entity configurations

- **Database and persistence**
  - EF Core migrations and database creation
  - SQL Server integration
  - Development connection string stored using .NET User Secrets
  - Unique constraints for customer email addresses and bank account numbers
  - Persistent customer, bank account, and account transaction storage
  - Generated bank account numbers
  - Initial bank account balances of zero
  - Account transaction table and foreign-key relationship
  - Account transaction indexes for account history and transaction ordering

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

- **Account transactions**
  - `AccountTransaction` domain entity
  - Deposit transaction type
  - Transaction identifiers using `Guid`
  - Transaction amount storage using `decimal`
  - Transaction creation timestamps
  - Balance snapshots after transactions
  - Transaction persistence through `BankDbContext`
  - EF Core migration for the account transaction table
  - Domain method for applying deposits to bank accounts

- **API and development tooling**
  - Swagger/OpenAPI support
  - RESTful API endpoints for customers and bank accounts
  - Feature-oriented application structure
  - Modular monolith architecture
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
├── bankoftrinh.client/                 # Angular frontend
│
├── BankOfTrinh.Server/                 # ASP.NET Core backend API
│   ├── Data/
│   │   ├── AccountTransactionConfigurations.cs
│   │   ├── BankAccountConfigurations.cs
│   │   ├── CustomerConfigurations.cs
│   │   ├── Migrations/
│   │   │   ├── ...                      # EF Core migrations
│   │   │   └── ...
│   │   └── BankDbContext.cs
│   │
│   ├── Domain/
│   │   ├── Accounts/
│   │   │   └── BankAccount.cs
│   │   ├── Customers/
│   │   │   └── Customer.cs
│   │   └── Transactions/
│   │       ├── AccountTransaction.cs
│   │       └── TransactionType.cs
│   │
│   ├── Features/
│   │   ├── Accounts/
│   │   │   ├── CreateBankAccount/
│   │   │   │   ├── CreateBankAccountEndpoint.cs
│   │   │   │   ├── CreateBankAccountRequest.cs
│   │   │   │   ├── CreateBankAccountResponse.cs
│   │   │   │   ├── CreateBankAccountService.cs
│   │   │   │   └── CustomerNotFoundException.cs
│   │   │   │
│   │   │   ├── Deposit/
│   │   │   │   ├── DepositEndpoint.cs
│   │   │   │   ├── DepositRequest.cs
│   │   │   │   ├── DepositResponse.cs
│   │   │   │   └── DepositService.cs
│   │   │   │
│   │   │   ├── GetBankAccount/
│   │   │   │   ├── GetBankAccountEndpoint.cs
│   │   │   │   ├── GetBankAccountRequest.cs
│   │   │   │   ├── GetBankAccountResponse.cs
│   │   │   │   ├── GetBankAccountService.cs
│   │   │   │   └── BankAccountNotFoundException.cs
│   │   │   │
│   │   │   └── GetCustomerBankAccounts/
│   │   │       ├── GetCustomerBankAccountsEndpoint.cs
│   │   │       ├── GetCustomerBankAccountsResponse.cs
│   │   │       └── GetCustomerBankAccountsService.cs
│   │   │
│   │   └── Customers/
│   │       ├── CreateCustomer/
│   │       │   ├── CreateCustomerEndpoint.cs
│   │       │   ├── CreateCustomerRequest.cs
│   │       │   ├── CreateCustomerResponse.cs
│   │       │   ├── CreateCustomerService.cs
│   │       │   └── CustomerEmailAlreadyExistsException.cs
│   │       │
│   │       └── GetCustomer/
│   │           ├── GetCustomerEndpoint.cs
│   │           ├── GetCustomerRequest.cs
│   │           ├── GetCustomerResponse.cs
│   │           └── GetCustomerService.cs
│   │
│   ├── Program.cs
│   └── appsettings.json
│
└── BankOfTrinh.Server.Tests/
    ├── Accounts/
    │   ├── CreateBankAccountTests.cs
    │   ├── DepositTests.cs
    │   ├── GetBankAccountTests.cs
    │   └── GetCustomerBankAccountsTests.cs
    │
    ├── Customers/
    │   ├── CreateCustomerTests.cs
    │   └── GetCustomerTests.cs
    │
    └── Infrastructure/
        ├── BankApiFactory.cs
        ├── DatabaseTestCollection.cs
        ├── SqlServerFixture.cs
        └── TestApiClient.cs
