using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petyei.Domain.Locations;

namespace Petyei.Infrastructure.Persistence.Configurations;

internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("countries");
        builder.HasKey(country => country.Code);
        builder.Property(country => country.Code).HasMaxLength(2).IsFixedLength();
        builder.HasIndex(country => country.IsEnabled);
    }
}

internal sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");
        builder.HasKey(department => department.Id);
        builder.Property(department => department.CountryCode).HasMaxLength(2).IsFixedLength();
        builder.Property(department => department.Code).HasMaxLength(10);
        builder.Property(department => department.Name).HasMaxLength(150);

        builder.HasOne<Country>().WithMany().HasForeignKey(department => department.CountryCode);
        builder.HasActiveUniqueIndex(department => new { department.CountryCode, department.Code });
    }
}

internal sealed class LocalityConfiguration : IEntityTypeConfiguration<Locality>
{
    public void Configure(EntityTypeBuilder<Locality> builder)
    {
        builder.ToTable("localities");
        builder.HasKey(locality => locality.Id);
        builder.Property(locality => locality.CountryCode).HasMaxLength(2).IsFixedLength();
        builder.Property(locality => locality.Code).HasMaxLength(10);
        builder.Property(locality => locality.Name).HasMaxLength(150);
        builder.Property(locality => locality.LocalityType).HasMaxLength(60);
        builder.Property(locality => locality.SourceVersion).HasMaxLength(40);

        builder.HasOne<Country>().WithMany().HasForeignKey(locality => locality.CountryCode);
        builder.HasOne<Department>().WithMany().HasForeignKey(locality => locality.DepartmentId);
        builder.HasActiveUniqueIndex(locality => new { locality.CountryCode, locality.Code });

        // Selector query: selectable localities of a country, grouped by department and ordered by name.
        builder.HasIndex(locality => new { locality.CountryCode, locality.IsSelectable, locality.DepartmentId, locality.Name });
    }
}
