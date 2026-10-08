# AGENTS.md

Instructions for AI coding tools working in this repository. Keep this file short and current.

## Project

BankOfTrinh is a banking simulator built for learning and portfolio practice, and the foundation for a planned payment processor (ACH first, then card, check scanning, and photo OCR intake). It is a modular monolith.

- `bankoftrinh.client` is the Angular frontend.
- `BankOfTrinh.Server` is the ASP.NET Core (net10.0) backend API, organized by feature folders (`Features/<Area>/<Feature>/` with endpoint, request, response, and service files).
- `BankOfTrinh.Ledger` is the double-entry ledger class library with its own `LedgerDbContext`.
- `BankOfTrinh.Ledger.Tests` holds the ledger unit tests (xUnit on in-memory SQLite).
- `BankOfTrinh.Server.Tests` holds the API integration tests (xUnit, `WebApplicationFactory`, SQL Server in Testcontainers, which needs Docker running).

The solution file is `BankOfTrinh.slnx`. Run all commands from the solution folder.

## Commands

```text
dotnet build
dotnet test BankOfTrinh.Ledger.Tests
dotnet test BankOfTrinh.Server.Tests
```

There are two DbContexts, so every EF command must name the context and the projects.

```text
# Ledger
dotnet ef migrations add <Name> --project BankOfTrinh.Ledger --startup-project BankOfTrinh.Server --context LedgerDbContext
dotnet ef database update --project BankOfTrinh.Ledger --startup-project BankOfTrinh.Server --context LedgerDbContext

# Existing app data
dotnet ef migrations add <Name> --project BankOfTrinh.Server --context BankDbContext
dotnet ef database update --project BankOfTrinh.Server --context BankDbContext
```

Local development uses SQL Server LocalDB (`(localdb)\MSSQLLocalDB`, database `BankOfTrinh`, connection string key `BankOfTrinh`). The Angular dev server runs on `https://localhost:56593` and needs a current Node.js LTS.

## Ledger rules (do not break these)

- Money is stored as whole cents in a `long`. Never use `float` or `double` for money.
- Every posting must balance: total debits equal total credits, with at least two lines and positive amounts.
- Every posting needs an idempotency key and a source document (`SourceType` and `SourceId`).
- The ledger is append-only. Never update or delete a saved `LedgerTransaction` or `LedgerEntry`. Correct mistakes with `IPostingService.ReverseAsync`.
- Account balances are derived from entries, not stored as a mutable field.
- Money-moving code goes through `IPostingService`, not direct inserts into ledger tables.
- The ledger uses the `ledger` schema and its own migrations history table. Do not mix its migrations with `BankDbContext` migrations.

## Conventions

- Keep the existing feature-folder structure. Add new features as new folders rather than growing existing services.
- Validate requests and return clear errors for missing or invalid input, matching the existing endpoints.
- Add or update tests with every behavior change. Prefer extending the existing test infrastructure (`TestApiClient`, `BankApiFactory`) over creating new helpers.
- Do not commit secrets. Connection strings with passwords belong in user secrets or environment variables, not in `appsettings.json`.
- Account numbers, routing numbers, and card data are sensitive. Do not log them in full.

## Before saying a task is done

- The solution builds.
- Relevant tests pass. State which tests you ran, and say so plainly if you could not run them.
- `README.md` is updated if features, commands, or structure changed.
