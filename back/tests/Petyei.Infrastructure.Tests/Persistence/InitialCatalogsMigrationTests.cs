using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using Npgsql;
using Petyei.Application.Common;
using Petyei.Domain.Localization;
using Petyei.Domain.Locations;
using Petyei.Domain.Pets;
using Petyei.Infrastructure.Persistence;

namespace Petyei.Infrastructure.Tests.Persistence;

/// <summary>Applies the real migrations to an empty PostgreSQL database and checks the resulting schema.</summary>
public sealed class InitialCatalogsMigrationTests(PostgresFixture database) : IClassFixture<PostgresFixture>, IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Migrations_LeaveNothingPendingAndMatchTheModel()
    {
        await using var context = CreateContext();

        Assert.Empty(await context.Database.GetPendingMigrationsAsync());
        Assert.False(context.Database.HasPendingModelChanges());
    }

    [Theory]
    [InlineData("ix_localities_country_code_code", "WHERE (state <> 2)")]
    [InlineData("ix_departments_country_code_code", "WHERE (state <> 2)")]
    [InlineData("ix_species_code", "WHERE (state <> 2)")]
    [InlineData("ix_breeds_species_id_code", "WHERE (state <> 2)")]
    [InlineData("ix_catalog_translations_entity_type_entity_key_field_locale", "WHERE (state <> 2)")]
    [InlineData("ix_localities_country_code_is_selectable_department_id_name", "(country_code, is_selectable, department_id, name)")]
    public async Task Migrations_CreateTheExpectedIndexes(string indexName, string expectedFragment)
    {
        var definition = await ScalarAsync<string>("SELECT indexdef FROM pg_indexes WHERE indexname = @name", indexName);

        Assert.Contains(expectedFragment, definition, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Migrations_RestrictDeletesOnEveryForeignKey()
    {
        var nonRestricted = await ScalarAsync<long>(
            "SELECT count(*) FROM information_schema.referential_constraints WHERE delete_rule <> 'RESTRICT' AND constraint_schema = 'public'");

        Assert.Equal(0, nonRestricted);
    }

    [Fact]
    public async Task Catalogs_StoreLocationsWithLeadingZerosAndTranslatedSpecies()
    {
        var colombia = Country.Create(CountryCode.Create("CO"), isEnabled: true);
        var atlantico = Department.Create(CountryCode.Create("CO"), "08", "ATLÁNTICO");
        var barranquilla = Locality.Create(atlantico, "08001", "BARRANQUILLA", "Municipio", "DANE-2024-12-30");
        var dog = Species.Create("dog", sortOrder: 1);
        var labrador = Breed.Create(dog, "labrador_retriever");

        await using (var context = CreateContext())
        {
            context.AddRange(colombia, atlantico, barranquilla, dog, labrador);
            context.AddRange(
                CatalogTranslation.Create("species", "dog", "name", "en", "Dog"),
                CatalogTranslation.Create("species", "dog", "name", "es", "Perro"));
            await context.SaveChangesAsync();
        }

        await using var check = CreateContext();
        var stored = await check.Set<Locality>().SingleAsync(locality => locality.Id == barranquilla.Id);
        Assert.Equal("08001", stored.Code);
        Assert.Equal(atlantico.Id, stored.DepartmentId);
        Assert.Equal("Perro", (await check.Set<CatalogTranslation>().SingleAsync(t => t.EntityKey == "dog" && t.Locale == "es")).Value);
    }

    [Fact]
    public async Task Catalogs_RejectALocalityForAnUnknownDepartment()
    {
        var ghost = Department.Create(CountryCode.Create("CO"), "99", "UNSAVED");
        await using var context = CreateContext();
        context.Add(Country.Create(CountryCode.Create("CO"), isEnabled: true));
        context.Add(Locality.Create(ghost, "99001", "NOWHERE", "Municipio", "test"));

        await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
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

    private async Task<T> ScalarAsync<T>(string sql, string? name = null)
    {
        await using var connection = new NpgsqlConnection(database.ConnectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        if (name is not null)
        {
            command.Parameters.AddWithValue("name", name);
        }

        return (T)(await command.ExecuteScalarAsync())!;
    }

    private sealed class NoActor : ICurrentActor
    {
        public Guid? AccountId => null;
    }
}
