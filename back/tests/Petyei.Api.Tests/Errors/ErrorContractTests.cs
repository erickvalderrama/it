using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Petyei.Application.Locations;

namespace Petyei.Api.Tests.Errors;

public sealed class ErrorContractTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task InvalidCountryCode_ReturnsValidationProblemWithStableCode()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/api/v1/locations/countries/C1", UriKind.Relative));
        var problem = await ReadProblemAsync(response);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("locations.invalid_country_code", problem.GetProperty("code").GetString());
        Assert.Equal("code", problem.GetProperty("field").GetString());
        Assert.False(string.IsNullOrEmpty(problem.GetProperty("correlationId").GetString()));
    }

    [Fact]
    public async Task DisabledCountry_ReturnsNotFoundWithStableCode()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/api/v1/locations/countries/EC", UriKind.Relative));
        var problem = await ReadProblemAsync(response);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("locations.country_not_enabled", problem.GetProperty("code").GetString());
    }

    [Fact]
    public async Task EnabledCountry_ReturnsTheCountry()
    {
        using var client = factory.CreateClient();

        var country = await client.GetFromJsonAsync<CountryDto>(new Uri("/api/v1/locations/countries/co", UriKind.Relative));

        Assert.Equal(new CountryDto("CO"), country);
    }

    [Fact]
    public async Task UnknownRoute_ReturnsCommonNotFoundCode()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/api/v1/unknown", UriKind.Relative));
        var problem = await ReadProblemAsync(response);

        Assert.Equal("common.not_found", problem.GetProperty("code").GetString());
    }

    [Fact]
    public async Task CorrelationId_IsEchoedWhenSafeAndReplacedWhenUnsafe()
    {
        using var client = factory.CreateClient();

        using var safe = new HttpRequestMessage(HttpMethod.Get, new Uri("/health/live", UriKind.Relative));
        safe.Headers.Add("X-Correlation-Id", "support-1234");
        using var safeResponse = await client.SendAsync(safe);

        using var unsafeRequest = new HttpRequestMessage(HttpMethod.Get, new Uri("/health/live", UriKind.Relative));
        unsafeRequest.Headers.Add("X-Correlation-Id", "<script>");
        using var unsafeResponse = await client.SendAsync(unsafeRequest);

        Assert.Equal("support-1234", safeResponse.Headers.GetValues("X-Correlation-Id").Single());
        Assert.Matches("^[0-9a-f]{32}$", unsafeResponse.Headers.GetValues("X-Correlation-Id").Single());
    }

    [Fact]
    public async Task Readiness_IsUnavailableWithoutDatabaseWhileLivenessIsOk()
    {
        using var client = factory.CreateClient();

        using var ready = await client.GetAsync(new Uri("/health/ready", UriKind.Relative));
        using var live = await client.GetAsync(new Uri("/health/live", UriKind.Relative));

        Assert.Equal(HttpStatusCode.ServiceUnavailable, ready.StatusCode);
        Assert.Equal(HttpStatusCode.OK, live.StatusCode);
    }

    [Fact]
    public async Task OpenApi_DescribesTheVersionedContractInDevelopment()
    {
        using var client = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Development")).CreateClient();

        var document = await client.GetStringAsync(new Uri("/openapi/v1.json", UriKind.Relative));

        Assert.Contains("/api/v1/locations/countries/{code}", document, StringComparison.Ordinal);
    }

    private static async Task<JsonElement> ReadProblemAsync(HttpResponseMessage response)
    {
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }
}
