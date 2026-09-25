# BankOfTrinh.Server

The backend API for BankOfTrinh, a banking simulator built for learning and portfolio practice.

The project is intentionally designed as a modular monolith. It is being used to practice C#, ASP.NET Core, Entity Framework Core, SQL Server, API design, database migrations, validation, and testing without overengineering the application.

## Current State

The initial database and domain setup is complete.

Implemented:

- `Customer` domain entity
- `BankAccount` domain entity
- `BankDbContext`
- SQL Server configuration
- Development connection string using .NET User Secrets
- Entity Framework Core entity configurations
- Initial database migration
- SQL Server database created through EF Core migrations
- Unique customer email constraint
- Customer creation feature
- Customer creation API endpoint
- Request validation
- Duplicate customer email handling
- Swagger/OpenAPI support
- Successful project build

## Project Structure

BankOfTrinh/
├── bankoftrinh.client/       # Angular frontend
└── BankOfTrinh.Server/       # ASP.NET Core backend API
    ├── Data/
    │   ├── BankAccountConfigurations.cs
    │   ├── CustomerConfigurations.cs
    │   ├── Migrations/
    │   └── BankDbContext.cs
    ├── Domain/
    │   ├── Accounts/
    │   │   └── BankAccount.cs
    │   └── Customers/
    │       └── Customer.cs
    ├── Features/
    │   └── Customers/
    │       └── CreateCustomer/
    │           ├── CreateCustomerEndpoint.cs
    │           ├── CreateCustomerRequest.cs
    │           ├── CreateCustomerResponse.cs
    │           ├── CreateCustomerService.cs
    │           └── CustomerEmailAlreadyExistsException.cs
    ├── Program.cs
    └── appsettings.json