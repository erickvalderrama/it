using System.Text.Json;
using Petyei.Application.Catalogs;

namespace Petyei.Api.Commands;

/// <summary>
/// Versioned catalog import: <c>dotnet Petyei.Api.dll import-catalogs [--data data/catalogs]</c>.
/// Reads <c>manifest.json</c>, validates every file, and only then writes. Safe to run again.
/// </summary>
internal static class ImportCatalogsCommand
{
    public const string Name = "import-catalogs";

    public static bool IsRequested(string[] args) => args.Length > 0 && args[0] == Name;

    public static async Task<int> RunAsync(IServiceProvider services, string[] args)
    {
        var dataIndex = Array.IndexOf(args, "--data");
        var dataDirectory = Path.GetFullPath(dataIndex >= 0 && dataIndex + 1 < args.Length ? args[dataIndex + 1] : Path.Combine("data", "catalogs"));
        if (!File.Exists(Path.Combine(dataDirectory, "manifest.json")))
        {
            await Console.Error.WriteLineAsync($"No manifest.json in {dataDirectory}. Pass --data <catalogs folder>.").ConfigureAwait(false);
            return 1;
        }

        var manifest = await ReadManifestAsync(dataDirectory).ConfigureAwait(false);

        var sources = new CatalogSources(
            await File.ReadAllTextAsync(Path.Combine(dataDirectory, manifest.Locations.Departments)).ConfigureAwait(false),
            await File.ReadAllTextAsync(Path.Combine(dataDirectory, manifest.Locations.Localities)).ConfigureAwait(false),
            manifest.Locations.SourceVersion,
            await File.ReadAllTextAsync(Path.Combine(dataDirectory, manifest.Pets.File)).ConfigureAwait(false));

        await using var scope = services.CreateAsyncScope();
        var result = await scope.ServiceProvider.GetRequiredService<ImportCatalogs>()
            .ExecuteAsync(sources, CancellationToken.None).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            await Console.Error.WriteLineAsync("Catalog import stopped; no data was written:").ConfigureAwait(false);
            foreach (var problem in result.Problems)
            {
                await Console.Error.WriteLineAsync($"  - {problem}").ConfigureAwait(false);
            }

            return 1;
        }

        Console.WriteLine($"Locations {manifest.Locations.SourceVersion}, pets {manifest.Pets.SourceVersion}");
        Console.WriteLine($"Localities by type: {string.Join(", ", result.LocalitiesByType.Select(t => $"{t.Key} {t.Value}"))}");
        foreach (var summary in result.Summaries)
        {
            Console.WriteLine(
                $"{summary.Catalog}: inserted {summary.Inserted}, updated {summary.Updated}, unchanged {summary.Unchanged}, missing from source {summary.MissingFromSource}");
        }

        return 0;
    }

    private static async Task<CatalogManifest> ReadManifestAsync(string dataDirectory)
    {
        await using var stream = File.OpenRead(Path.Combine(dataDirectory, "manifest.json"));
        return await JsonSerializer.DeserializeAsync<CatalogManifest>(stream, JsonSerializerOptions.Web).ConfigureAwait(false)
            ?? throw new InvalidOperationException("The catalog manifest is empty.");
    }

    private sealed record CatalogManifest(LocationFiles Locations, PetFiles Pets);

    private sealed record LocationFiles(string SourceVersion, string Departments, string Localities);

    private sealed record PetFiles(string SourceVersion, string File);
}
