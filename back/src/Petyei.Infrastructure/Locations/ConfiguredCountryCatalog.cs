using Microsoft.Extensions.Options;
using Petyei.Application.Locations;
using Petyei.Domain.Locations;

namespace Petyei.Infrastructure.Locations;

/// <summary>Reads enabled countries from configuration until the location catalog is persisted.</summary>
internal sealed class ConfiguredCountryCatalog(IOptions<CountryCatalogOptions> options) : ICountryCatalog
{
    public Task<IReadOnlyList<CountryCode>> GetEnabledAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<CountryCode> codes = [.. options.Value.Enabled.Select(CountryCode.Create)];
        return Task.FromResult(codes);
    }
}
