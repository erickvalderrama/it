using Petyei.Application.Locations;

namespace Petyei.Application.Tests.Locations;

public sealed class PlaceNameFormatterTests
{
    [Theory]
    [InlineData("SANTA MARTA", "Santa Marta")]
    [InlineData("EL CARMEN DE BOLÍVAR", "El Carmen de Bolívar")]
    [InlineData("BOGOTÁ, D.C.", "Bogotá, D.C.")]
    [InlineData("ARCHIPIÉLAGO DE SAN ANDRÉS, PROVIDENCIA Y SANTA CATALINA", "Archipiélago de San Andrés, Providencia y Santa Catalina")]
    [InlineData("SAN JOSÉ DEL GUAVIARE", "San José del Guaviare")]
    [InlineData("LA JAGUA DE IBIRICO", "La Jagua de Ibirico")]
    public void ToDisplayName_UsesSpanishTitleCase(string official, string expected)
    {
        Assert.Equal(expected, PlaceNameFormatter.ToDisplayName(official));
    }
}
