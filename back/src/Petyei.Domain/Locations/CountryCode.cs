namespace Petyei.Domain.Locations;

/// <summary>ISO 3166-1 alpha-2 country code, stored in upper case.</summary>
public readonly record struct CountryCode
{
    private CountryCode(string value) => Value = value;

    public string Value { get; }

    public static bool TryCreate(string? input, out CountryCode code)
    {
        var normalized = input?.Trim().ToUpperInvariant();
        if (normalized is { Length: 2 } && normalized.All(char.IsAsciiLetterUpper))
        {
            code = new CountryCode(normalized);
            return true;
        }

        code = default;
        return false;
    }

    public static CountryCode Create(string? input) =>
        TryCreate(input, out var code)
            ? code
            : throw new ArgumentException("Country code must be two ASCII letters.", nameof(input));

    public override string ToString() => Value;
}
