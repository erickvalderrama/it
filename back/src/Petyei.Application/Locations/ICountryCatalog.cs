using Petyei.Domain.Locations;

namespace Petyei.Application.Locations;

/// <summary>Source of the countries where Petyei operates.</summary>
public interface ICountryCatalog
{
    Task<IReadOnlyList<CountryCode>> GetEnabledAsync(CancellationToken cancellationToken);
}
