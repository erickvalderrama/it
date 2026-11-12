using Testcontainers.PostgreSql;

namespace Petyei.Infrastructure.Tests.Persistence;

/// <summary>Disposable PostgreSQL with PostGIS for persistence tests. Requires Docker.</summary>
public sealed class PostgresFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer container = new PostgreSqlBuilder("postgis/postgis:17-3.5")
        .WithDatabase("petyei_test")
        .Build();

    public string ConnectionString => container.GetConnectionString();

    public Task InitializeAsync() => container.StartAsync();

    public Task DisposeAsync() => container.DisposeAsync().AsTask();
}
