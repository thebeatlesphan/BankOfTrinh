# BankOfTrinh.Server

The backend API for BankOfTrinh, a banking simulator built for learning and portfolio practice.

The project is intentionally designed as a modular monolith. It is being used to practice C#, ASP.NET Core, Entity Framework Core, SQL Server, API design, database migrations, validation, integration testing, and automated testing without overengineering the application.

## Current Status

The initial database, domain model, customer management, bank account management, validation, and automated integration test infrastructure are in place.

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
  - Create and retrieve bank accounts through API endpoints
  - Association of new accounts with existing customers
  - Missing-customer and missing-account handling

- **API and development tooling**
  - Swagger/OpenAPI support
  - Successful project build
  - Feature-oriented application structure

- **Automated testing**
  - Dedicated xUnit test project
  - ASP.NET Core integration testing with `WebApplicationFactory`
  - SQL Server test database provisioned with Testcontainers
  - Test database migrations using the application's EF Core migrations
  - Test configuration that overrides the development connection string
  - Coverage for validation, API responses, persistence, uniqueness constraints, retrieval, and customer-account associations

The recommended next feature is retrieving all bank accounts belonging to a customer.

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
│   │   │   └── GetBankAccount/
│   │   │       └── BankAccountNotFoundException.cs
│   │   │       ├── GetBankAccountEndpoint.cs
│   │   │       ├── GetBankAccountRequest.cs
│   │   │       ├── GetBankAccountResponse.cs
│   │   │       └── GetBankAccountService.cs
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
    │   └── GetBankAccountTests.cs
    ├── Customers/
    │   ├── CreateCustomerTests.cs
    │   └── GetCustomerTests.cs
    └── Infrastructure/
        ├── BankApiFactory.cs
        ├── DatabaseTestCollection.cs
        └── SqlServerFixture.cs

The recommended next feature is retrieving all bank accounts belonging to a customer.