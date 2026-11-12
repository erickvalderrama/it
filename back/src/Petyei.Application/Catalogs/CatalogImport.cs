namespace Petyei.Application.Catalogs;

/// <summary>Counts reported by an import. Records missing from the source are reported, never deleted.</summary>
public sealed record CatalogImportSummary(string Catalog, int Inserted, int Updated, int Unchanged, int MissingFromSource);

/// <summary>Writes validated catalog data, matching existing records by their stable code.</summary>
public interface ICatalogStore
{
    Task<IReadOnlyList<CatalogImportSummary>> UpsertLocationsAsync(
        LocationCatalogData data,
        string sourceVersion,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<CatalogImportSummary>> UpsertPetCatalogAsync(PetCatalogData data, CancellationToken cancellationToken);
}

public sealed record CatalogImportResult(
    IReadOnlyList<CatalogImportSummary> Summaries,
    IReadOnlyList<string> Problems,
    IReadOnlyDictionary<string, int> LocalitiesByType)
{
    public bool Succeeded => Problems.Count == 0;
}

/// <summary>
/// Validates both catalog sources first and only writes when both are valid, so a bad file never
/// leaves a partial update.
/// </summary>
public sealed class ImportCatalogs(ICatalogStore store)
{
    public async Task<CatalogImportResult> ExecuteAsync(CatalogSources sources, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(sources);
        var (locations, locationProblems) = LocationCatalogParser.Parse(sources.DepartmentsCsv, sources.LocalitiesCsv);
        var (pets, petProblems) = PetCatalogParser.Parse(sources.SpeciesBreedsCsv);
        var problems = locationProblems.Concat(petProblems).ToList();
        if (locations is null || pets is null || problems.Count > 0)
        {
            return new CatalogImportResult([], problems, new Dictionary<string, int>());
        }

        var summaries = new List<CatalogImportSummary>();
        summaries.AddRange(await store.UpsertLocationsAsync(locations, sources.LocationSourceVersion, cancellationToken).ConfigureAwait(false));
        summaries.AddRange(await store.UpsertPetCatalogAsync(pets, cancellationToken).ConfigureAwait(false));
        return new CatalogImportResult(summaries, [], locations.LocalitiesByType);
    }
}

public sealed record CatalogSources(
    string DepartmentsCsv,
    string LocalitiesCsv,
    string LocationSourceVersion,
    string SpeciesBreedsCsv);
