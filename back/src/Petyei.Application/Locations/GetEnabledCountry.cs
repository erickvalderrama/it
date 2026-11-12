using Petyei.Application.Common;
using Petyei.Domain.Locations;

namespace Petyei.Application.Locations;

/// <summary>Returns one enabled country, used to validate the country chosen at registration.</summary>
public sealed class GetEnabledCountry(ICountryCatalog catalog)
{
    public async Task<Result<CountryDto>> ExecuteAsync(string? code, CancellationToken cancellationToken)
    {
        if (!CountryCode.TryCreate(code, out var countryCode))
        {
            return Result.Failure<CountryDto>(new AppError(ErrorCodes.InvalidCountryCode, ErrorKind.Validation, "code"));
        }

        var enabled = await catalog.GetEnabledAsync(cancellationToken).ConfigureAwait(false);
        return enabled.Contains(countryCode)
            ? Result.Success(new CountryDto(countryCode.Value))
            : Result.Failure<CountryDto>(new AppError(ErrorCodes.CountryNotEnabled, ErrorKind.NotFound));
    }
}
