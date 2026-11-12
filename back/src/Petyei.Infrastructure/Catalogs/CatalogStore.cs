using Microsoft.EntityFrameworkCore;
using Petyei.Application.Catalogs;
using Petyei.Domain.Localization;
using Petyei.Domain.Locations;
using Petyei.Domain.Pets;
using Petyei.Infrastructure.Persistence;

namespace Petyei.Infrastructure.Catalogs;

/// <summary>
/// Upserts catalogs by stable code inside one transaction per catalog. Existing records keep their
/// identifiers, so accounts and listings that reference them are preserved. Nothing is deleted.
/// </summary>
internal sealed class CatalogStore(PetyeiDbContext db) : ICatalogStore
{
    private const string NameField = "name";

    public async Task<IReadOnlyList<CatalogImportSummary>> UpsertLocationsAsync(
        LocationCatalogData data,
        string sourceVersion,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(data);
        var countryCode = data.Country.Value;
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        if (!await db.Set<Country>().AnyAsync(c => c.Code == countryCode, cancellationToken).ConfigureAwait(false))
        {
            // Importing a country's catalog enables it; administrators can disable it later.
            db.Add(Country.Create(data.Country, isEnabled: true));
        }

        var departments = await db.Set<Department>().Where(d => d.CountryCode == countryCode)
            .ToDictionaryAsync(d => d.Code, StringComparer.Ordinal, cancellationToken).ConfigureAwait(false);
        var departmentCounts = new Counter("departments");
        foreach (var row in data.Departments)
        {
            if (departments.TryGetValue(row.Code, out var existing))
            {
                departmentCounts.Track(existing.Name != row.Name, () => existing.Rename(row.Name));
                continue;
            }

            var created = Department.Create(data.Country, row.Code, row.Name);
            db.Add(created);
            departments[row.Code] = created;
            departmentCounts.Inserted++;
        }

        var localities = await db.Set<Locality>().Where(l => l.CountryCode == countryCode)
            .ToDictionaryAsync(l => l.Code, StringComparer.Ordinal, cancellationToken).ConfigureAwait(false);
        var localityCounts = new Counter("localities");
        foreach (var row in data.Localities)
        {
            if (localities.TryGetValue(row.Code, out var existing))
            {
                var changed = existing.Name != row.Name || existing.LocalityType != row.LocalityType || existing.SourceVersion != sourceVersion;
                localityCounts.Track(changed, () => existing.UpdateFromSource(row.Name, row.LocalityType, sourceVersion));
                continue;
            }

            db.Add(Locality.Create(departments[row.DepartmentCode], row.Code, row.Name, row.LocalityType, sourceVersion));
            localityCounts.Inserted++;
        }

        departmentCounts.MissingFromSource = departments.Keys.Except(data.Departments.Select(d => d.Code)).Count();
        localityCounts.MissingFromSource = localities.Keys.Except(data.Localities.Select(l => l.Code)).Count();

        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        return [departmentCounts.ToSummary(), localityCounts.ToSummary()];
    }

    public async Task<IReadOnlyList<CatalogImportSummary>> UpsertPetCatalogAsync(PetCatalogData data, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(data);
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        var species = await db.Set<Species>().ToDictionaryAsync(s => s.Code, StringComparer.Ordinal, cancellationToken).ConfigureAwait(false);
        var breeds = await db.Set<Breed>().ToListAsync(cancellationToken).ConfigureAwait(false);
        var translations = await db.Set<CatalogTranslation>()
            .Where(t => (t.EntityType == "species" || t.EntityType == "breed") && t.Field == NameField)
            .ToDictionaryAsync(t => (t.EntityType, t.EntityKey, t.Locale), cancellationToken).ConfigureAwait(false);

        var speciesCounts = new Counter("species");
        var breedCounts = new Counter("breeds");
        var translationCounts = new Counter("catalog_translations");

        foreach (var row in data.Species)
        {
            if (species.TryGetValue(row.Code, out var existing))
            {
                speciesCounts.Track(existing.SortOrder != row.SortOrder, () => existing.SetSortOrder(row.SortOrder));
            }
            else
            {
                existing = Species.Create(row.Code, row.SortOrder);
                db.Add(existing);
                species[row.Code] = existing;
                speciesCounts.Inserted++;
            }

            UpsertNames("species", row.Code, row.Names, translations, translationCounts);
        }

        var breedsByKey = breeds.ToDictionary(b => (b.SpeciesId, b.Code));
        foreach (var row in data.Breeds)
        {
            var parent = species[row.SpeciesCode];
            if (breedsByKey.ContainsKey((parent.Id, row.Code)))
            {
                breedCounts.Unchanged++;
            }
            else
            {
                db.Add(Breed.Create(parent, row.Code));
                breedCounts.Inserted++;
            }

            UpsertNames("breed", $"{row.SpeciesCode}.{row.Code}", row.Names, translations, translationCounts);
        }

        speciesCounts.MissingFromSource = species.Keys.Except(data.Species.Select(s => s.Code)).Count();
        var sourceBreeds = data.Breeds.Select(b => (species[b.SpeciesCode].Id, b.Code)).ToHashSet();
        breedCounts.MissingFromSource = breeds.Count(b => !sourceBreeds.Contains((b.SpeciesId, b.Code)));

        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        return [speciesCounts.ToSummary(), breedCounts.ToSummary(), translationCounts.ToSummary()];
    }

    private void UpsertNames(
        string entityType,
        string entityKey,
        LocalizedNames names,
        Dictionary<(string, string, string), CatalogTranslation> translations,
        Counter counts)
    {
        foreach (var (locale, value) in new[] { ("en", names.English), ("es", names.Spanish) })
        {
            if (translations.TryGetValue((entityType, entityKey, locale), out var existing))
            {
                counts.Track(existing.Value != value, () => existing.ChangeValue(value));
                continue;
            }

            var created = CatalogTranslation.Create(entityType, entityKey, NameField, locale, value);
            db.Add(created);
            translations[(entityType, entityKey, locale)] = created;
            counts.Inserted++;
        }
    }

    private sealed class Counter(string catalog)
    {
        public int Inserted { get; set; }

        public int Updated { get; private set; }

        public int Unchanged { get; set; }

        public int MissingFromSource { get; set; }

        public void Track(bool changed, Action apply)
        {
            if (!changed)
            {
                Unchanged++;
                return;
            }

            apply();
            Updated++;
        }

        public CatalogImportSummary ToSummary() => new(catalog, Inserted, Updated, Unchanged, MissingFromSource);
    }
}
