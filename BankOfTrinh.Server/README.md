# BankOfTrinh.Server

The backend API for BankOfTrinh, a banking simulator built for learning and portfolio practice.

This project is intentionally designed as a modular monolith. The goal is to practice C#, ASP.NET Core, Entity Framework Core, SQL Server, Docker, API design, transactions, testing, and general systems design without overengineering the application.

## Current Progress

The initial domain structure has been created.

Currently implemented:

- Domain folder structure
- `Accounts` domain folder
- `Customers` domain folder
- Placeholder locations for account and customer models

Not implemented yet:

- Customer model
- Bank account model
- Entity Framework Core `DbContext`
- Database configuration
- API endpoints
- Migrations
- Authentication
- Banking transactions
- Automated tests
- Docker configuration

## Repository Structure

```text
BankOfTrinh/
├── bankoftrinh.client/       # Angular frontend
└── BankOfTrinh.server/       # ASP.NET Core backend API
    ├── Data/                 # EF Core DbContext, configurations, and migrations
    ├── Domain/               # Core business entities and rules
    │   ├── Accounts/         # Bank account-related domain models
    │   └── Customers/        # Customer-related domain models
    ├── Features/             # Application features organized by use case
    ├── Common/               # Shared errors, results, and utilities
    ├── Program.cs            # Application startup and dependency registration
    └── appsettings.json      # Non-secret application configuration