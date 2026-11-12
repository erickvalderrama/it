using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Petyei.Application.Locations;
using Petyei.Domain.Locations;

namespace Petyei.Api.Tests;

/// <summary>
/// Runs the API in a "Testing" environment so developer user-secrets and local settings never leak
/// into tests, and replaces the catalog ports with in-memory data. Database behavior is covered by
/// the Infrastructure tests.
/// </summary>
public sealed class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ICountryCatalog>();
            services.RemoveAll<ILocalityCatalog>();
            services.AddSingleton<InMemoryLocationCatalog>();
            services.AddSingleton<ICountryCatalog>(provider => provider.GetRequiredService<InMemoryLocationCatalog>());
            services.AddSingleton<ILocalityCatalog>(provider => provider.GetRequiredService<InMemoryLocationCatalog>());
        });
    }
}

internal sealed class InMemoryLocationCatalog : ICountryCatalog, ILocalityCatalog
{
    private static readonly LocalityRecord[] Localities =
    [
        new(Guid.Parse("01930000-0000-7000-8000-000000000001"), "47001", "SANTA MARTA", "47", "MAGDALENA"),
        new(Guid.Parse("01930000-0000-7000-8000-000000000002"), "47053", "ARACATACA", "47", "MAGDALENA"),
        new(Guid.Parse("01930000-0000-7000-8000-000000000003"), "08001", "BARRANQUILLA", "08", "ATLÁNTICO"),
        new(Guid.Parse("01930000-0000-7000-8000-000000000004"), "11001", "BOGOTÁ, D.C.", "11", "BOGOTÁ, D.C."),
    ];

    public Task<IReadOnlyList<CountryCode>> GetEnabledAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<CountryCode>>([CountryCode.Create("CO")]);

    public Task<IReadOnlyList<LocalityRecord>> GetSelectableAsync(CountryCode country, CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<LocalityRecord>>(country.Value == "CO" ? Localities : []);

    public Task<LocalityRecord?> FindSelectableAsync(CountryCode country, string localityCode, CancellationToken cancellationToken) =>
        Task.FromResult(country.Value == "CO" ? Localities.FirstOrDefault(l => l.Code == localityCode) : null);
}
