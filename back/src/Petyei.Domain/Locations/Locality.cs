using Petyei.Domain.Common;

namespace Petyei.Domain.Locations;

/// <summary>
/// Municipality or equivalent territory (DIVIPOLA locality). Accounts and listings reference it by
/// identifier; catalog updates change it by official code so those references are preserved.
/// </summary>
public sealed class Locality : SoftDeletableEntity
{
    private Locality()
    {
    }

    public Guid Id { get; private set; }

    public Guid DepartmentId { get; private set; }

    public string CountryCode { get; private set; } = string.Empty;

    /// <summary>Official code kept as text to preserve leading zeros (DIVIPOLA "08001").</summary>
    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    /// <summary>Territory type from the source, such as "Municipio" or "Isla".</summary>
    public string LocalityType { get; private set; } = string.Empty;

    /// <summary>Whether the locality is offered in selectors. Managed separately from the source data.</summary>
    public bool IsSelectable { get; private set; }

    /// <summary>Source release that last updated this record, for traceability of catalog updates.</summary>
    public string SourceVersion { get; private set; } = string.Empty;

    public static Locality Create(
        Department department,
        string code,
        string name,
        string localityType,
        string sourceVersion)
    {
        ArgumentNullException.ThrowIfNull(department);
        if (string.IsNullOrWhiteSpace(code) || !code.StartsWith(department.Code, StringComparison.Ordinal))
        {
            throw new ArgumentException("The locality code must start with its department code.", nameof(code));
        }

        return new Locality
        {
            Id = Guid.CreateVersion7(),
            DepartmentId = department.Id,
            CountryCode = department.CountryCode,
            Code = code,
            Name = name.Trim(),
            LocalityType = localityType.Trim(),
            IsSelectable = true,
            SourceVersion = sourceVersion,
        };
    }

    public void UpdateFromSource(string name, string localityType, string sourceVersion)
    {
        Name = name.Trim();
        LocalityType = localityType.Trim();
        SourceVersion = sourceVersion;
    }

    public void SetSelectable(bool isSelectable) => IsSelectable = isSelectable;
}
