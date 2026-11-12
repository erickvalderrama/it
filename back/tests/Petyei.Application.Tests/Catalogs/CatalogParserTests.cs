using Petyei.Application.Catalogs;
using Petyei.Application.Common;

namespace Petyei.Application.Tests.Catalogs;

public sealed class CatalogParserTests
{
    private const string Departments = "country_code,department_code,department_name\n\"CO\",\"08\",\"ATLÁNTICO\"\n\"CO\",\"11\",\"BOGOTÁ, D.C.\"\n";

    private const string Localities =
        "country_code,department_code,locality_code,locality_name,locality_type\n" +
        "\"CO\",\"08\",\"08001\",\"BARRANQUILLA\",\"Municipio\"\n" +
        "\"CO\",\"11\",\"11001\",\"BOGOTÁ, D.C.\",\"Municipio\"\n";

    [Fact]
    public void Csv_ReadsQuotedCommasEscapedQuotesAndBom()
    {
        var records = CsvParser.ReadRecords("﻿a,b\r\n\"x, y\",\"say \"\"hi\"\"\"\r\n");

        Assert.Equal("x, y", records[0]["a"]);
        Assert.Equal("say \"hi\"", records[0]["b"]);
    }

    [Fact]
    public void Locations_KeepLeadingZerosAndCountTypes()
    {
        var (data, problems) = LocationCatalogParser.Parse(Departments, Localities);

        Assert.Empty(problems);
        Assert.Equal("CO", data!.Country.Value);
        Assert.Equal("08001", data.Localities[0].Code);
        Assert.Equal("BOGOTÁ, D.C.", data.Departments[1].Name);
        Assert.Equal(2, data.LocalitiesByType["Municipio"]);
    }

    [Fact]
    public void Locations_ReportDuplicatesMissingDepartmentsAndPrefixErrors()
    {
        var localities = Localities +
            "\"CO\",\"08\",\"08001\",\"DUPLICATE\",\"Municipio\"\n" +
            "\"CO\",\"47\",\"47001\",\"SANTA MARTA\",\"Municipio\"\n" +
            "\"CO\",\"08\",\"11999\",\"WRONG PREFIX\",\"Municipio\"\n";

        var (data, problems) = LocationCatalogParser.Parse(Departments, localities);

        Assert.Null(data);
        Assert.Contains("Duplicate locality code 08001.", problems);
        Assert.Contains("Locality 47001 references missing department 47.", problems);
        Assert.Contains("Locality 11999 does not start with its department code 08.", problems);
    }

    [Fact]
    public void Locations_RejectMissingColumns()
    {
        var (_, problems) = LocationCatalogParser.Parse("country_code,department_code\n\"CO\",\"08\"\n", Localities);

        Assert.Contains("The departments file is missing the column 'department_name'.", problems);
    }

    [Fact]
    public void Pets_ReadSpeciesAndBreedsWithBothLanguages()
    {
        const string csv =
            "kind,species_code,breed_code,sort_order,name_en,name_es\n" +
            "species,dog,,1,Dog,Perro\n" +
            "breed,dog,mixed_breed,,Mixed breed,Criollo\n";

        var (data, problems) = PetCatalogParser.Parse(csv);

        Assert.Empty(problems);
        Assert.Equal(new SpeciesRow("dog", 1, new LocalizedNames("Dog", "Perro")), data!.Species.Single());
        Assert.Equal(new BreedRow("dog", "mixed_breed", new LocalizedNames("Mixed breed", "Criollo")), data.Breeds.Single());
    }

    [Fact]
    public void Pets_ReportInvalidCodesMissingNamesAndOrphanBreeds()
    {
        const string csv =
            "kind,species_code,breed_code,sort_order,name_en,name_es\n" +
            "species,dog,,1,Dog,\n" +
            "breed,cat,siamese,,Siamese,Siamés\n" +
            "breed,dog,Bad Code,,Bad,Malo\n";

        var (data, problems) = PetCatalogParser.Parse(csv);

        Assert.Null(data);
        Assert.Contains("Row 2 needs English and Spanish names.", problems);
        Assert.Contains("Breed siamese references missing species cat.", problems);
        Assert.Contains("Row 4 has an invalid breed code 'Bad Code'.", problems);
    }

    [Fact]
    public async Task Import_WritesNothingWhenAnyFileIsInvalid()
    {
        var store = new RecordingStore();

        var result = await new ImportCatalogs(store).ExecuteAsync(
            new CatalogSources(Departments, Localities, "v1", "kind,species_code\n"),
            CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.False(store.Called);
    }

    private sealed class RecordingStore : ICatalogStore
    {
        public bool Called { get; private set; }

        public Task<IReadOnlyList<CatalogImportSummary>> UpsertLocationsAsync(LocationCatalogData data, string sourceVersion, CancellationToken cancellationToken)
        {
            Called = true;
            return Task.FromResult<IReadOnlyList<CatalogImportSummary>>([]);
        }

        public Task<IReadOnlyList<CatalogImportSummary>> UpsertPetCatalogAsync(PetCatalogData data, CancellationToken cancellationToken)
        {
            Called = true;
            return Task.FromResult<IReadOnlyList<CatalogImportSummary>>([]);
        }
    }
}
