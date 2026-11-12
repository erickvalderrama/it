using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petyei.Domain.Common;
using Petyei.Domain.Localization;
using Petyei.Domain.Pets;

namespace Petyei.Infrastructure.Persistence.Configurations;

internal sealed class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");
        builder.HasKey(species => species.Id);
        builder.Property(species => species.Code).HasMaxLength(CatalogCode.MaxLength);
        builder.HasActiveUniqueIndex(species => species.Code);
        builder.HasIndex(species => species.SortOrder);
    }
}

internal sealed class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");
        builder.HasKey(breed => breed.Id);
        builder.Property(breed => breed.Code).HasMaxLength(CatalogCode.MaxLength);
        builder.HasOne<Species>().WithMany().HasForeignKey(breed => breed.SpeciesId);
        builder.HasActiveUniqueIndex(breed => new { breed.SpeciesId, breed.Code });
    }
}

internal sealed class CatalogTranslationConfiguration : IEntityTypeConfiguration<CatalogTranslation>
{
    public void Configure(EntityTypeBuilder<CatalogTranslation> builder)
    {
        builder.ToTable("catalog_translations");
        builder.HasKey(translation => translation.Id);
        builder.Property(translation => translation.EntityType).HasMaxLength(60);
        builder.Property(translation => translation.EntityKey).HasMaxLength(150);
        builder.Property(translation => translation.Field).HasMaxLength(60);
        builder.Property(translation => translation.Locale).HasMaxLength(10);
        builder.Property(translation => translation.Value).HasMaxLength(CatalogTranslation.MaxValueLength);

        builder.HasActiveUniqueIndex(translation => new
        {
            translation.EntityType,
            translation.EntityKey,
            translation.Field,
            translation.Locale,
        });

        // Loading one catalog in one language for selectors and API responses.
        builder.HasIndex(translation => new { translation.EntityType, translation.Locale });
    }
}
