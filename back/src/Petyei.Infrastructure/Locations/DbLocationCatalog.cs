using Microsoft.EntityFrameworkCore;
using Petyei.Application.Locations;
using Petyei.Domain.Locations;
using Petyei.Infrastructure.Persistence;

namespace Petyei.Infrastructure.Locations;

/// <summary>Countries and localities read from the imported catalog. Deleted rows are excluded by the global filter.</summary>
internal sealed class DbLocationCatalog(PetyeiDbContext db) : ICountryCatalog, ILocalityCatalog
{
    public async Task<IReadOnlyList<CountryCode>> GetEnabledAsync(CancellationToken cancellationToken)
    {
        var codes = await db.Set<Country>().AsNoTracking()
            .Where(country => country.IsEnabled)
            .Select(country => country.Code)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
        return [.. codes.Select(CountryCode.Create)];
    }

    public async Task<IReadOnlyList<LocalityRecord>> GetSelectableAsync(CountryCode country, CancellationToken cancellationToken) =>
        await Selectable(country, localityCode: null)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

    public Task<LocalityRecord?> FindSelectableAsync(CountryCode country, string localityCode, CancellationToken cancellationToken) =>
        Selectable(country, localityCode).FirstOrDefaultAsync(cancellationToken);

    // Filters and ordering run on the entities; the projection to LocalityRecord comes last so EF can translate it.
    private IQueryable<LocalityRecord> Selectable(CountryCode country, string? localityCode)
    {
        var code = country.Value;
        var query = from locality in db.Set<Locality>().AsNoTracking()
                    join department in db.Set<Department>().AsNoTracking() on locality.DepartmentId equals department.Id
                    where locality.CountryCode == code && locality.IsSelectable
                    select new { locality, department };
        if (localityCode is not null)
        {
            query = query.Where(row => row.locality.Code == localityCode);
        }

        return query
            .OrderBy(row => row.department.Name).ThenBy(row => row.locality.Name)
            .Select(row => new LocalityRecord(row.locality.Id, row.locality.Code, row.locality.Name, row.department.Code, row.department.Name));
    }
}
