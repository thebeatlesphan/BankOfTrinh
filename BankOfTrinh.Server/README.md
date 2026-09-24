# BankOfTrinh.Server

The backend API for BankOfTrinh, a banking simulator built for learning and portfolio practice.

This project is intentionally designed as a modular monolith. The goal is to practice C#, ASP.NET Core, Entity Framework Core, SQL Server, Docker, API design, transactions, testing, and general systems design without overengineering the application.

## Repository Structure

```text
BankOfTrinh/
├── bankoftrinh.client/       # Angular frontend
└── BankOfTrinh.server/       # ASP.NET Core backend API
    ├── Data/                 # EF Core DbContext, configurations, and migrations
    ├── Domain/               # Core business entities and rules
    ├── Features/             # Application features organized by use case
    ├── Common/               # Shared errors, results, and utilities
    ├── Program.cs            # Application startup and dependency registration
    └── appsettings.json      # Non-secret application configuration