using Petyei.Domain.Common;

namespace Petyei.Domain.Localization;

/// <summary>
/// Display text of a catalog field in one language, keyed by entity type, entity code, field, and
/// locale. English is the fallback when a translation is missing.
/// </summary>
public sealed class CatalogTranslation : SoftDeletableEntity
{
    public const int MaxValueLength = 500;

    private CatalogTranslation()
    {
    }

    public Guid Id { get; private set; }

    /// <summary>Catalog name, such as "species" or "breed".</summary>
    public string EntityType { get; private set; } = string.Empty;

    /// <summary>Stable key of the record, such as "dog" or "dog.labrador_retriever".</summary>
    public string EntityKey { get; private set; } = string.Empty;

    public string Field { get; private set; } = string.Empty;

    /// <summary>BCP 47 language tag, such as "en" or "es".</summary>
    public string Locale { get; private set; } = string.Empty;

    public string Value { get; private set; } = string.Empty;

    public static CatalogTranslation Create(string entityType, string entityKey, string field, string locale, string value) => new()
    {
        Id = Guid.CreateVersion7(),
        EntityType = entityType,
        EntityKey = entityKey,
        Field = field,
        Locale = locale,
        Value = RequireValue(value),
    };

    public void ChangeValue(string value) => Value = RequireValue(value);

    private static string RequireValue(string value) =>
        string.IsNullOrWhiteSpace(value) || value.Length > MaxValueLength
            ? throw new ArgumentException($"A translation needs 1 to {MaxValueLength} characters.", nameof(value))
            : value.Trim();
}
