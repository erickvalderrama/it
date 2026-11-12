using Petyei.Application.Common;
using Petyei.Domain.Locations;

namespace Petyei.Application.Locations;

public sealed record LocalityDto(Guid Id, string Code, string Name, string DisplayName);

public sealed record DepartmentLocalitiesDto(string Code, string Name, string DisplayName, IReadOnlyList<LocalityDto> Localities);

/// <summary>Localities for the city picker, grouped by department.</summary>
public sealed class ListSelectableLocalities(ICountryCatalog countries, ILocalityCatalog localities)
{
    public async Task<Result<IReadOnlyList<DepartmentLocalitiesDto>>> ExecuteAsync(string? countryCode, CancellationToken cancellationToken)
    {
        var country = await EnabledCountry.ResolveAsync(countries, countryCode, cancellationToken).ConfigureAwait(false);
        if (country.Error is not null)
        {
            return Result.Failure<IReadOnlyList<DepartmentLocalitiesDto>>(country.Error);
        }

        var records = await localities.GetSelectableAsync(country.Value, cancellationToken).ConfigureAwait(false);
        IReadOnlyList<DepartmentLocalitiesDto> groups =
        [
            .. records
                .GroupBy(r => (r.DepartmentCode, r.DepartmentName))
                .Select(g => new DepartmentLocalitiesDto(
                    g.Key.DepartmentCode,
                    g.Key.DepartmentName,
                    PlaceNameFormatter.ToDisplayName(g.Key.DepartmentName),
                    [.. g.Select(ToDto).OrderBy(l => l.DisplayName, StringComparer.CurrentCulture)]))
                .OrderBy(g => g.DisplayName, StringComparer.CurrentCulture),
        ];
        return Result.Success(groups);
    }

    internal static LocalityDto ToDto(LocalityRecord record) =>
        new(record.Id, record.Code, record.Name, PlaceNameFormatter.ToDisplayName(record.Name));
}

/// <summary>Server-side check of a chosen locality, used by registration and listings.</summary>
public sealed class GetSelectableLocality(ICountryCatalog countries, ILocalityCatalog localities)
{
    public async Task<Result<LocalityDto>> ExecuteAsync(string? countryCode, string? localityCode, CancellationToken cancellationToken)
    {
        var country = await EnabledCountry.ResolveAsync(countries, countryCode, cancellationToken).ConfigureAwait(false);
        if (country.Error is not null)
        {
            return Result.Failure<LocalityDto>(country.Error);
        }

        var code = localityCode?.Trim() ?? string.Empty;
        var record = code.Length is > 0 and <= 10
            ? await localities.FindSelectableAsync(country.Value, code, cancellationToken).ConfigureAwait(false)
            : null;
        return record is null
            ? Result.Failure<LocalityDto>(new AppError(ErrorCodes.LocalityNotAvailable, ErrorKind.Validation, "localityCode"))
            : Result.Success(ListSelectableLocalities.ToDto(record));
    }
}

internal static class EnabledCountry
{
    public static async Task<Result<CountryCode>> ResolveAsync(ICountryCatalog countries, string? code, CancellationToken cancellationToken)
    {
        if (!CountryCode.TryCreate(code, out var country))
        {
            return Result.Failure<CountryCode>(new AppError(ErrorCodes.InvalidCountryCode, ErrorKind.Validation, "countryCode"));
        }

        var enabled = await countries.GetEnabledAsync(cancellationToken).ConfigureAwait(false);
        return enabled.Contains(country)
            ? Result.Success(country)
            : Result.Failure<CountryCode>(new AppError(ErrorCodes.CountryNotEnabled, ErrorKind.NotFound));
    }
}
