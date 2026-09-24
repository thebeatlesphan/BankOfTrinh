# Local Docker Setup

This directory runs the local SQL Server instance for BankOfTrinh.

## Prerequisites

- Docker Desktop
- Docker Compose

## First-time setup

Create `.env` in this directory:

```env
MSSQL_SA_PASSWORD=LocalDevPassword_123!
```

Use a strong local-only password. `.env` is ignored by Git.

## Start SQL Server

From the repository root:

```bash
docker compose \
  --env-file infra/docker/.env \
  -f infra/docker/compose.dev.yml \
  up -d
```

Check the container:

```bash
docker compose \
  --env-file infra/docker/.env \
  -f infra/docker/compose.dev.yml \
  ps
```

View logs:

```bash
docker compose \
  --env-file infra/docker/.env \
  -f infra/docker/compose.dev.yml \
  logs -f sqlserver
```

## Local connection

From the host machine:

```text
Server=localhost,1433
Database=BankOfTrinh
User Id=sa
Password=<MSSQL_SA_PASSWORD>
TrustServerCertificate=True
```

When the API is running inside Docker, use:

```text
Server=sqlserver,1433
```

`sqlserver` is the Compose service name.

## Stop SQL Server

Stop the container and preserve the database:

```bash
docker compose \
  --env-file infra/docker/.env \
  -f infra/docker/compose.dev.yml \
  down
```

Reset the database completely:

```bash
docker compose \
  --env-file infra/docker/.env \
  -f infra/docker/compose.dev.yml \
  down -v
```

Use `down -v` only when I intentionally want to delete the local database.

## Notes

- SQL Server uses the Developer edition for local development.
- Do not commit `.env`.
- The `sqlserver-data` volume preserves data between restarts.
- EF Core migrations will be applied from the ASP.NET Core project.