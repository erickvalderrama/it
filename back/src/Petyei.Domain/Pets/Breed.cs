using Petyei.Domain.Common;

namespace Petyei.Domain.Pets;

/// <summary>Breed of a species (thesis table Breeds). Display names come from catalog translations.</summary>
public sealed class Breed : SoftDeletableEntity
{
    private Breed()
    {
    }

    public Guid Id { get; private set; }

    public Guid SpeciesId { get; private set; }

    /// <summary>Stable English key unique within its species, such as "labrador_retriever".</summary>
    public string Code { get; private set; } = string.Empty;

    public static Breed Create(Species species, string code)
    {
        ArgumentNullException.ThrowIfNull(species);
        return new Breed
        {
            Id = Guid.CreateVersion7(),
            SpeciesId = species.Id,
            Code = CatalogCode.Require(code),
        };
    }
}
