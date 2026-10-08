# BankOfTrinh.Server

The backend API for BankOfTrinh, a banking simulator built for learning and portfolio practice.

The project is intentionally designed as a modular monolith. It is being used to practice C#, ASP.NET Core, Entity Framework Core, SQL Server, API design, database migrations, validation, integration testing, and automated testing without overengineering the application.

## Current Status

The backend currently supports customer management, bank account management, deposits, transaction persistence, validation, and automated integration testing.

Customers can be created and retrieved, bank accounts can be created for customers, and deposits update account balances while creating persisted account transactions.

A double-entry ledger library (`BankOfTrinh.Ledger`) has been added as the foundation for a future payment processor. The library and its unit tests are written and the solution builds. The ledger migration has not been applied yet, the ledger tests have not been run yet, and `DepositService` is not connected to the ledger yet.

The integration test suite for the existing features contains 18 passing tests.

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
- SQL Server integration (SQL Server LocalDB for local development)
- Development connection string `BankOfTrinh` configured in `appsettings.Development.json`
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

### Double-Entry Ledger (`BankOfTrinh.Ledger`)

A separate class library that records money movement as immutable, balanced entries. It is the planned foundation for payment intake (ACH first, then card, check, and photo OCR).

- Separate `LedgerDbContext` using the `ledger` schema and its own migrations history table (`__EFMigrationsHistory_Ledger`), so it does not collide with `BankDbContext` migrations
- `Account`, `LedgerTransaction`, and `LedgerEntry` entities
- Amounts stored as whole cents (`long`), USD only for now
- Debit and credit directions with account types (asset, liability, revenue, expense)
- `IPostingService`: posts balanced transactions, rejects unbalanced or invalid postings, derives account balances, and reports a trial balance
- Idempotency keys so retried postings are safe; the same key with different content is rejected
- Required source document reference (`SourceType` and `SourceId`) on every transaction
- Reversals through mirrored transactions; a transaction can be reversed once
- Append-only enforcement: saved transactions and entries cannot be updated or deleted
- Database constraints: unique idempotency key, unique account code, positive-amount check constraint
- `IAccountService` for get-or-create of ledger accounts
- `AddLedger(connectionString)` extension for registration in `Program.cs`

### API and Development Tooling

- Swagger/OpenAPI support
- RESTful API endpoints for customers and bank accounts
- Feature-oriented application structure
- Modular monolith architecture
- Development database configuration through `appsettings.Development.json`
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
- Separate `BankOfTrinh.Ledger.Tests` project (xUnit on in-memory SQLite, which enforces unique indexes, foreign keys, check constraints, and transactions) covering balanced and unbalanced postings, invalid amounts, unknown accounts, idempotent replay and conflicts, reversals, append-only enforcement, and trial balance

## Planned Work

- Apply the ledger migration (`InitialLedger`) to the local database
- Run and confirm the ledger test suite
- Connect `DepositService` to the ledger
- Payments API (`POST /v1/payments` with idempotency keys) and an ACH virtual terminal in the Angular client
- NACHA file generation, batching, and return-file handling
- Card payments through a partner acquirer or gateway, then check scanning and photo OCR intake

## Running Locally

- Open the solution in Visual Studio and use **Configure Startup Projects > Multiple startup projects** with `bankoftrinh.client` and `BankOfTrinh.Server` both set to **Start**.
- The Angular dev server runs on `https://localhost:56593`. It requires a current Node.js version (22 LTS or newer).
- The database is SQL Server LocalDB (`(localdb)\MSSQLLocalDB`, database `BankOfTrinh`).

Apply ledger migrations (run from the solution folder):

```text
dotnet ef migrations add InitialLedger --project BankOfTrinh.Ledger --startup-project BankOfTrinh.Server --context LedgerDbContext
dotnet ef database update --project BankOfTrinh.Ledger --startup-project BankOfTrinh.Server --context LedgerDbContext
```

Run the ledger tests:

```text
dotnet test BankOfTrinh.Ledger.Tests
```

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
├── BankOfTrinh.Ledger/                 # Double-entry ledger library
│   ├── AccountService.cs
│   ├── Entities.cs                     # Account, LedgerTransaction, LedgerEntry
│   ├── Exceptions.cs
│   ├── LedgerDbContext.cs
│   ├── Migrations/                     # Created when InitialLedger is generated
│   ├── PostingService.cs
│   └── ServiceCollectionExtensions.cs  # AddLedger(...)
│
├── BankOfTrinh.Ledger.Tests/
│   └── PostingServiceTests.cs
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
```