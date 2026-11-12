using Petyei.Domain.Common;

namespace Petyei.Domain.Locations;

/// <summary>First-level division of a country, such as a Colombian department.</summary>
public sealed class Department : SoftDeletableEntity
{
    private Department()
    {
    }

    public Guid Id { get; private set; }

    public string CountryCode { get; private set; } = string.Empty;

    /// <summary>Official code kept as text to preserve leading zeros (DIVIPOLA "08").</summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Official name as published by the source.</summary>
    public string Name { get; private set; } = string.Empty;

    public static Department Create(CountryCode country, string code, string name) => new()
    {
        Id = Guid.CreateVersion7(),
        CountryCode = country.Value,
        Code = RequireText(code, nameof(code)),
        Name = RequireText(name, nameof(name)),
    };

    public void Rename(string name) => Name = RequireText(name, nameof(name));

    private static string RequireText(string value, string parameter) =>
        string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("A value is required.", parameter) : value.Trim();
}
