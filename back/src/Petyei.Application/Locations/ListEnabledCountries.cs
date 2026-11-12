namespace Petyei.Application.Locations;

public sealed record CountryDto(string Code);

/// <summary>Lists the countries available for registration and listings, in a stable order.</summary>
public sealed class ListEnabledCountries(ICountryCatalog catalog)
{
    public async Task<IReadOnlyList<CountryDto>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var codes = await catalog.GetEnabledAsync(cancellationToken).ConfigureAwait(false);
        return [.. codes.Distinct().OrderBy(code => code.Value, StringComparer.Ordinal).Select(code => new CountryDto(code.Value))];
    }
}
