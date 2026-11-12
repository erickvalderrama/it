using Petyei.Application.Common;
using Petyei.Application.Locations;
using Petyei.Domain.Locations;

namespace Petyei.Application.Tests.Locations;

public sealed class GetEnabledCountryTests
{
    private readonly GetEnabledCountry useCase = new(new SingleCountryCatalog());

    [Fact]
    public async Task ExecuteAsync_ReturnsTheNormalizedEnabledCountry()
    {
        var result = await useCase.ExecuteAsync(" co ", CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(new CountryDto("CO"), result.Value);
    }

    [Fact]
    public async Task ExecuteAsync_RejectsAMalformedCodeAsValidation()
    {
        var result = await useCase.ExecuteAsync("Colombia", CancellationToken.None);

        Assert.Equal(new AppError(ErrorCodes.InvalidCountryCode, ErrorKind.Validation, "code"), result.Error);
    }

    [Fact]
    public async Task ExecuteAsync_ReportsADisabledCountryAsNotFound()
    {
        var result = await useCase.ExecuteAsync("EC", CancellationToken.None);

        Assert.Equal(new AppError(ErrorCodes.CountryNotEnabled, ErrorKind.NotFound), result.Error);
    }

    private sealed class SingleCountryCatalog : ICountryCatalog
    {
        public Task<IReadOnlyList<CountryCode>> GetEnabledAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<CountryCode>>([CountryCode.Create("CO")]);
    }
}
