using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace Petyei.Api.Tests.Errors;

public sealed class ReadinessWithDatabaseTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer database = new PostgreSqlBuilder("postgis/postgis:17-3.5").Build();

    public Task InitializeAsync() => database.StartAsync();

    public Task DisposeAsync() => database.DisposeAsync().AsTask();

    [Fact]
    public async Task Readiness_IsOkWhenTheDatabaseIsReachable()
    {
        using var factory = new ApiFactory().WithWebHostBuilder(builder =>
            builder.UseSetting("ConnectionStrings:Petyei", database.GetConnectionString()));
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/health/ready", UriKind.Relative));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
