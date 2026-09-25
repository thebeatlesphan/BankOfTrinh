# BankOfTrinh.Server

The backend API for BankOfTrinh, a banking simulator built for learning and portfolio practice.

This project is intentionally designed as a modular monolith. The goal is to practice C#, ASP.NET Core, Entity Framework Core, SQL Server, Docker, API design, transactions, testing, and general systems design without overengineering the application.

## Current Progress

The initial domain entities have been created:

- `Customer`
- `BankAccount`

These are currently plain C# domain classes. They contain the initial banking data and business rules but are not connected to a database or API yet.

## Repository Structure

```text
BankOfTrinh/
├── bankoftrinh.client/       # Angular frontend
└── BankOfTrinh.server/       # ASP.NET Core backend API
    ├── Data/                 # EF Core DbContext, configurations, and migrations
    ├── Domain/               # Core business entities and rules
    │   ├── Accounts/
    │   │   └── BankAccount.cs
    │   └── Customers/
    │       └── Customer.cs
    ├── Features/             # Application features organized by use case
    ├── Common/               # Shared errors, results, and utilities
    ├── Program.cs            # Application startup and dependency registration
    └── appsettings.json      # Non-secret application configuration