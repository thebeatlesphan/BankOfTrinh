# BankOfTrinh.Server

The backend API for BankOfTrinh, a banking simulator built for learning and portfolio practice.

The project is intentionally designed as a modular monolith. It is being used to practice C#, ASP.NET Core, Entity Framework Core, SQL Server, API design, database migrations, validation, integration testing, and automated testing without overengineering the application.

## Current Status

The backend currently supports customer management, bank account management, deposits, transaction persistence, validation, and automated integration testing.

Customers can be created and retrieved, bank accounts can be created for customers, and deposits update account balances while creating persisted account transactions.

The project builds successfully and the automated test suite currently contains 18 passing tests.

## Implemented Features

### Domain and Data Model

- `Customer` entity
- `BankAccount` entity
- `AccountTransaction` entity
- `TransactionType` enum
- Customer-to-bank-account relationship
- Bank-account-to-transaction relationship
- Account balance management through domain methods
- Transaction balance snapshots using `BalanceAfterTransaction`
- `BankDbContext`
- EF Core entity configurations

### Database and Persistence

- EF Core migrations and database creation
- SQL Server integration
- Development connection string stored using .NET User Secrets
- Unique constraints for customer email addresses and bank account numbers
- Persistent customer, bank account, and account transaction storage
- Generated bank account numbers
- Initial bank account balances of zero
- Account transaction table with a foreign-key relationship to bank accounts
- Account transaction indexes for account history and transaction ordering
- Persistent transaction identifiers using `Guid`
- Transaction amount storage using `decimal`
- Transaction creation timestamps

### Customer Management

- Create customers through an API endpoint
- Retrieve customers through an API endpoint
- Request validation
- Duplicate email handling
- Missing-customer handling

### Bank Account Management

- Create bank accounts for existing customers
- Retrieve individual bank accounts
- Retrieve all bank accounts belonging to a customer
- Associate new bank accounts with existing customers
- Missing-customer handling
- Missing-account handling
- Initial account balance of zero
- Generated bank account numbers

### Account Transactions

- Deposit transaction type
- Deposit API endpoint
- Deposit request and response models
- Deposit service
- Domain method for applying deposits to bank accounts
- Account balance updates
- Balance snapshots after deposits
- Transaction persistence through `BankDbContext`
- Transaction identifiers using `Guid`
- Validation for non-positive deposit amounts
- Missing bank account handling for deposits

### API and Development Tooling

- Swagger/OpenAPI support
- RESTful API endpoints for customers and bank accounts
- Feature-oriented application structure
- Modular monolith architecture
- Development database configuration using User Secrets
- Successful project build

### Automated Testing

- Dedicated xUnit test project
- ASP.NET Core integration testing with `WebApplicationFactory`
- SQL Server test database provisioned with Testcontainers
- Test database migrations using the application's EF Core migrations
- Test configuration that overrides the development connection string
- Reusable `TestApiClient` for common API operations
- Customer creation and retrieval tests
- Bank account creation and retrieval tests
- Tests for retrieving all bank accounts for a customer
- Validation failure tests
- Duplicate customer email tests
- Missing customer tests
- Missing bank account tests
- Database persistence tests
- Customer-account association tests
- Unique bank account number tests
- Valid deposit integration test
- Verification that deposits update account balances
- Verification of deposit response values
- Negative deposit validation test
- Missing bank account deposit test
- Tests confirming invalid deposits do not update account balances

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
