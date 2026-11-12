using Petyei.Domain.Common;

namespace Petyei.Domain.Pets;

/// <summary>Animal type (thesis table Animal_Types). Display names come from catalog translations.</summary>
public sealed class Species : SoftDeletableEntity
{
    private Species()
    {
    }

    public Guid Id { get; private set; }

    /// <summary>Stable English key, such as "dog" or "guinea_pig".</summary>
    public string Code { get; private set; } = string.Empty;

    public int SortOrder { get; private set; }

    public static Species Create(string code, int sortOrder) => new()
    {
        Id = Guid.CreateVersion7(),
        Code = CatalogCode.Require(code),
        SortOrder = sortOrder,
    };

    public void SetSortOrder(int sortOrder) => SortOrder = sortOrder;
}
