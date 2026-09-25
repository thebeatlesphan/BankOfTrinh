## Current Progress

The initial domain entities and EF Core database setup have been created.

Implemented:

- `Customer` domain entity
- `BankAccount` domain entity
- `BankDbContext`
- SQL Server provider configuration
- Development connection string using .NET User Secrets
- Initial EF Core migration: `InitialCreate`

The initial migration has been generated but has not yet been applied to the SQL Server database.

## Repository Structure

```text
BankOfTrinh/
├── bankoftrinh.client/       # Angular frontend
└── BankOfTrinh.server/       # ASP.NET Core backend API
    ├── Data/
    │   ├── Configurations/   # EF Core entity configurations
    │   ├── Migrations/       # EF Core database migrations
    │   └── BankDbContext.cs  # EF Core database context
    ├── Domain/               # Core business entities and rules
    │   ├── Accounts/
    │   │   └── BankAccount.cs
    │   └── Customers/
    │       └── Customer.cs
    ├── Features/             # Application features organized by use case
    ├── Common/               # Shared errors, results, and utilities
    ├── Program.cs            # Application startup and dependency registration
    └── appsettings.json      # Non-secret application configuration