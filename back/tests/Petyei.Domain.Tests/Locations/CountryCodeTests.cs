using Petyei.Domain.Locations;

namespace Petyei.Domain.Tests.Locations;

public sealed class CountryCodeTests
{
    [Theory]
    [InlineData("CO", "CO")]
    [InlineData(" co ", "CO")]
    [InlineData("Es", "ES")]
    public void TryCreate_NormalizesValidCodes(string input, string expected)
    {
        Assert.True(CountryCode.TryCreate(input, out var code));
        Assert.Equal(expected, code.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("C")]
    [InlineData("COL")]
    [InlineData("C1")]
    [InlineData("Ñ0")]
    public void TryCreate_RejectsInvalidCodes(string? input)
    {
        Assert.False(CountryCode.TryCreate(input, out _));
    }

    [Fact]
    public void Create_ThrowsForInvalidCode()
    {
        Assert.Throws<ArgumentException>(() => CountryCode.Create("Colombia"));
    }
}
