# BankOfTrinh.Server

The backend API for BankOfTrinh, a banking simulator built for learning and portfolio practice.

This project is intentionally designed as a modular monolith. The goal is to practice C#, ASP.NET Core, Entity Framework Core, SQL Server, Docker, API design, transactions, testing, and general systems design without overengineering the application.

## Current Progress

The initial domain and database setup has been completed.

Implemented:

- `Customer` domain entity
- `BankAccount` domain entity
- `BankDbContext`
- SQL Server provider configuration
- Development connection string using .NET User Secrets
- Entity Framework Core entity configuration
- Initial EF Core migration: `InitialCreate`
- SQL Server database created through EF Core migrations
- Initial database tables created successfully

The application currently builds successfully. Visual Studio IntelliSense/autocomplete is not currently working correctly, but this does not prevent the project from building or running.

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
