using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using Petyei.Application.Catalogs;
using Petyei.Application.Common;
using Petyei.Domain.Localization;
using Petyei.Domain.Locations;
using Petyei.Infrastructure.Catalogs;
using Petyei.Infrastructure.Locations;
using Petyei.Infrastructure.Persistence;
using Petyei.Infrastructure.Tests.Persistence;

namespace Petyei.Infrastructure.Tests.Catalogs;

/// <summary>Imports the real versioned catalog files into a migrated PostgreSQL database.</summary>
public sealed class CatalogStoreTests(PostgresFixture database) : IClassFixture<PostgresFixture>, IAsyncLifetime
{
    private static readonly string DataDirectory = Path.Combine(AppContext.BaseDirectory, "catalogs");

    public async Task InitializeAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Import_LoadsEveryCatalogFromTheVersionedFiles()
    {
        var result = await ImportAsync("DANE-2024-12-30");

        Assert.True(result.Succeeded, string.Join("; ", result.Problems));
        Assert.Equal(1103, result.LocalitiesByType["Municipio"]);
        Assert.Equal(18, result.LocalitiesByType["Área no municipalizada"]);
        AssertInserted(result, "departments", 33);
        AssertInserted(result, "localities", 1122);
        AssertInserted(result, "species", 20);
        AssertInserted(result, "breeds", 121);
        AssertInserted(result, "catalog_translations", 282);

        await using var context = CreateContext();
        var santaMarta = await context.Set<Locality>().SingleAsync(l => l.Code == "47001");
        Assert.Equal("SANTA MARTA", santaMarta.Name);
        Assert.True((await context.Set<Country>().SingleAsync()).IsEnabled);
        Assert.Equal("Cobaya (cuyo)", (await context.Set<CatalogTranslation>()
            .SingleAsync(t => t.EntityKey == "guinea_pig" && t.Locale == "es")).Value);
    }

    [Fact]
    public async Task Reimport_KeepsIdentifiersAndPilotSelectionAndChangesNothing()
    {
        await ImportAsync("DANE-2024-12-30");
        Guid barranquillaId;
        await using (var context = CreateContext())
        {
            var barranquilla = await context.Set<Locality>().SingleAsync(l => l.Code == "08001");
            barranquilla.SetSelectable(false);
            barranquillaId = barranquilla.Id;
            await context.SaveChangesAsync();
        }

        var again = await ImportAsync("DANE-2024-12-30");

        Assert.All(again.Summaries, summary =>
        {
            Assert.Equal(0, summary.Inserted);
            Assert.Equal(0, summary.Updated);
        });
        await using var check = CreateContext();
        var stored = await check.Set<Locality>().SingleAsync(l => l.Code == "08001");
        Assert.Equal(barranquillaId, stored.Id);
        Assert.False(stored.IsSelectable);
    }

    [Fact]
    public async Task Reimport_WithANewSourceVersionUpdatesLocalitiesInPlace()
    {
        await ImportAsync("DANE-2024-12-30");

        var result = await ImportAsync("DANE-2025-06-30");

        Assert.Equal(1122, result.Summaries.Single(s => s.Catalog == "localities").Updated);
        await using var context = CreateContext();
        Assert.Equal(1122, await context.Set<Locality>().CountAsync(l => l.SourceVersion == "DANE-2025-06-30"));
    }

    [Fact]
    public async Task LocationCatalog_ServesSelectableLocalitiesAndHidesDisabledOnes()
    {
        await ImportAsync("DANE-2024-12-30");
        var colombia = CountryCode.Create("CO");

        await using (var context = CreateContext())
        {
            var catalog = new DbLocationCatalog(context);
            Assert.Equal([colombia], await catalog.GetEnabledAsync(CancellationToken.None));
            Assert.Equal(1122, (await catalog.GetSelectableAsync(colombia, CancellationToken.None)).Count);
            var santaMarta = await catalog.FindSelectableAsync(colombia, "47001", CancellationToken.None);
            Assert.Equal("MAGDALENA", santaMarta!.DepartmentName);

            (await context.Set<Locality>().SingleAsync(l => l.Code == "47053")).SetSelectable(false);
            await context.SaveChangesAsync();
        }

        await using var check = CreateContext();
        var reloaded = new DbLocationCatalog(check);
        Assert.Null(await reloaded.FindSelectableAsync(colombia, "47053", CancellationToken.None));
        Assert.Equal(1121, (await reloaded.GetSelectableAsync(colombia, CancellationToken.None)).Count);
    }

    private static void AssertInserted(CatalogImportResult result, string catalog, int expected) =>
        Assert.Equal(expected, result.Summaries.Single(s => s.Catalog == catalog).Inserted);

    private async Task<CatalogImportResult> ImportAsync(string locationVersion)
    {
        var sources = new CatalogSources(
            await File.ReadAllTextAsync(Path.Combine(DataDirectory, "divipola", "colombia-departments.csv")),
            await File.ReadAllTextAsync(Path.Combine(DataDirectory, "divipola", "colombia-divipola-localities.csv")),
            locationVersion,
            await File.ReadAllTextAsync(Path.Combine(DataDirectory, "species-breeds.csv")));
        await using var context = CreateContext();
        return await new ImportCatalogs(new CatalogStore(context)).ExecuteAsync(sources, CancellationToken.None);
    }

    private PetyeiDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PetyeiDbContext>()
            .UseNpgsql(database.ConnectionString)
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(new SoftDeleteInterceptor(new NoActor(), new FakeTimeProvider()))
            .Options;
        return new PetyeiDbContext(options);
    }

    private sealed class NoActor : ICurrentActor
    {
        public Guid? AccountId => null;
    }
}
