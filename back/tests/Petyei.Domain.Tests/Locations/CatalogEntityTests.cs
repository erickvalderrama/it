using Petyei.Domain.Common;
using Petyei.Domain.Localization;
using Petyei.Domain.Locations;
using Petyei.Domain.Pets;

namespace Petyei.Domain.Tests.Locations;

public sealed class CatalogEntityTests
{
    private static readonly Department Magdalena = Department.Create(CountryCode.Create("CO"), "47", "MAGDALENA");

    [Fact]
    public void Locality_TakesCountryAndDepartmentFromItsDepartment()
    {
        var santaMarta = Locality.Create(Magdalena, "47001", " SANTA MARTA ", "Municipio", "DANE-2024-12-30");

        Assert.Equal("CO", santaMarta.CountryCode);
        Assert.Equal(Magdalena.Id, santaMarta.DepartmentId);
        Assert.Equal("SANTA MARTA", santaMarta.Name);
        Assert.True(santaMarta.IsSelectable);
    }

    [Fact]
    public void Locality_RejectsACodeFromAnotherDepartment()
    {
        Assert.Throws<ArgumentException>(() => Locality.Create(Magdalena, "08001", "BARRANQUILLA", "Municipio", "v1"));
    }

    [Theory]
    [InlineData("dog")]
    [InlineData("guinea_pig")]
    [InlineData("mini_pig2")]
    public void CatalogCode_AcceptsStableKeys(string code)
    {
        Assert.Equal(code, Species.Create(code, 1).Code);
    }

    [Theory]
    [InlineData("Dog")]
    [InlineData("guinea pig")]
    [InlineData("perro-labrador")]
    [InlineData("_dog")]
    public void CatalogCode_RejectsUnstableKeys(string code)
    {
        Assert.Throws<ArgumentException>(() => CatalogCode.Require(code));
    }

    [Fact]
    public void Breed_BelongsToItsSpecies()
    {
        var cat = Species.Create("cat", 2);

        Assert.Equal(cat.Id, Breed.Create(cat, "siamese").SpeciesId);
    }

    [Fact]
    public void CatalogTranslation_RequiresText()
    {
        Assert.Throws<ArgumentException>(() => CatalogTranslation.Create("species", "dog", "name", "es", "  "));
    }
}
