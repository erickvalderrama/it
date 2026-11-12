using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Petyei.Application.Locations;

namespace Petyei.Api.Tests.Locations;

public sealed class LocalityEndpointsTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task ListLocalities_GroupsByDepartmentWithDisplayNames()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/api/v1/locations/countries/CO/localities", UriKind.Relative));
        var groups = await response.Content.ReadFromJsonAsync<List<DepartmentLocalitiesDto>>() ?? [];

        Assert.Equal("public, max-age=3600", response.Headers.CacheControl?.ToString());
        Assert.Equal(["Atlántico", "Bogotá, D.C.", "Magdalena"], groups.Select(g => g.DisplayName));
        var magdalena = groups[2];
        Assert.Equal(["Aracataca", "Santa Marta"], magdalena.Localities.Select(l => l.DisplayName));
        Assert.Equal("47001", magdalena.Localities[1].Code);
        Assert.Equal("SANTA MARTA", magdalena.Localities[1].Name);
    }

    [Fact]
    public async Task ListLocalities_ForADisabledCountryReturnsNotFound()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(new Uri("/api/v1/locations/countries/EC/localities", UriKind.Relative));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("locations.country_not_enabled", await CodeAsync(response));
    }

    [Fact]
    public async Task GetLocality_AcceptsASelectableLocality()
    {
        using var client = factory.CreateClient();

        var locality = await client.GetFromJsonAsync<LocalityDto>(new Uri("/api/v1/locations/countries/co/localities/47001", UriKind.Relative));

        Assert.Equal("Santa Marta", locality!.DisplayName);
    }

    [Theory]
    [InlineData("99999")]
    [InlineData("12345678901")]
    public async Task GetLocality_RejectsAnUnknownOrDisabledLocality(string code)
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(new Uri($"/api/v1/locations/countries/CO/localities/{code}", UriKind.Relative));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("locations.locality_not_available", await CodeAsync(response));
    }

    private static async Task<string?> CodeAsync(HttpResponseMessage response) =>
        (await response.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("code").GetString();
}
