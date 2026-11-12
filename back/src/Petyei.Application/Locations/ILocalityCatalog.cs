using Petyei.Domain.Locations;

namespace Petyei.Application.Locations;

public sealed record LocalityRecord(Guid Id, string Code, string Name, string DepartmentCode, string DepartmentName);

/// <summary>Read access to the localities people can choose.</summary>
public interface ILocalityCatalog
{
    /// <summary>Selectable localities of a country, ordered by department and name.</summary>
    Task<IReadOnlyList<LocalityRecord>> GetSelectableAsync(CountryCode country, CancellationToken cancellationToken);

    Task<LocalityRecord?> FindSelectableAsync(CountryCode country, string localityCode, CancellationToken cancellationToken);
}
