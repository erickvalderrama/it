using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Petyei.Application.Locations;

namespace Petyei.Api.Tests.Locations;

public sealed class LocationEndpointsTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task ListCountries_ReturnsColombiaFromConfiguration()
    {
        using var client = factory.CreateClient();

        var countries = await client.GetFromJsonAsync<List<CountryDto>>(new Uri("/api/v1/locations/countries", UriKind.Relative));

        Assert.Equal([new CountryDto("CO")], countries);
    }

    [Fact]
    public async Task Liveness_ReturnsOk()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/health/live", UriKind.Relative));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UnknownRoute_ReturnsProblemDetails()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/api/v1/unknown", UriKind.Relative));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Cors_RejectsUnlistedOrigin()
    {
        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/api/v1/locations/countries", UriKind.Relative));
        request.Headers.Add("Origin", "https://evil.example");

        using var response = await client.SendAsync(request);

        Assert.False(response.Headers.Contains("Access-Control-Allow-Origin"));
    }
}
