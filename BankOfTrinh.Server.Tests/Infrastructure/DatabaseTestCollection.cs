namespace BankOfTrinh.Server.Tests.Infrastructure;

[CollectionDefinition("Database collection")]
public sealed class DatabaseTestCollection
    : ICollectionFixture<SqlServerFixture>
{
}