using Petyei.Application.Locations;
using Petyei.Domain.Locations;

namespace Petyei.Application.Tests.Locations;

public sealed class ListEnabledCountriesTests
{
    [Fact]
    public async Task ExecuteAsync_ReturnsDistinctCodesInStableOrder()
    {
        var catalog = new FakeCountryCatalog(CountryCode.Create("EC"), CountryCode.Create("CO"), CountryCode.Create("CO"));

        var result = await new ListEnabledCountries(catalog).ExecuteAsync(CancellationToken.None);

        Assert.Equal([new CountryDto("CO"), new CountryDto("EC")], result);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsEmptyListWhenNoCountryIsEnabled()
    {
        var result = await new ListEnabledCountries(new FakeCountryCatalog()).ExecuteAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    private sealed class FakeCountryCatalog(params CountryCode[] codes) : ICountryCatalog
    {
        public Task<IReadOnlyList<CountryCode>> GetEnabledAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<CountryCode>>(codes);
    }
}
