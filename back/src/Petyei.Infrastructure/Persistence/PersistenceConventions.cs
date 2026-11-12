using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petyei.Domain.Common;

namespace Petyei.Infrastructure.Persistence;

/// <summary>Project-wide persistence rules applied to every model.</summary>
public static class PersistenceConventions
{
    /// <summary>SQL filter for rows that are not logically deleted, used by partial indexes.</summary>
    public const string ActiveRowFilter = "state <> 2";

    /// <summary>
    /// Hides deleted records by default, stores the state as an indexed integer, and restricts
    /// deletes on every foreign key so no cascade can remove rows.
    /// </summary>
    public static ModelBuilder ApplyPlatformConventions(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var foreignKey in entityType.GetForeignKeys())
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            var clrType = entityType.ClrType;
            if (!typeof(SoftDeletableEntity).IsAssignableFrom(clrType) || entityType.BaseType is not null)
            {
                continue;
            }

            var entity = modelBuilder.Entity(clrType);
            entity.Property(nameof(SoftDeletableEntity.State)).HasConversion<int>();
            entity.Property(nameof(SoftDeletableEntity.DeletionReason)).HasMaxLength(500);
            entity.Ignore(nameof(SoftDeletableEntity.IsDeleted));
            entity.HasIndex(nameof(SoftDeletableEntity.State));
            entity.HasQueryFilter(ActiveFilter(clrType));
        }

        return modelBuilder;
    }

    /// <summary>Unique index that only applies to active rows, so a deleted record does not block reuse.</summary>
    public static IndexBuilder<T> HasActiveUniqueIndex<T>(this EntityTypeBuilder<T> builder, Expression<Func<T, object?>> columns)
        where T : SoftDeletableEntity
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.HasIndex(columns).IsUnique().HasFilter(ActiveRowFilter);
    }

    private static LambdaExpression ActiveFilter(Type clrType)
    {
        var parameter = Expression.Parameter(clrType, "record");
        var state = Expression.Property(parameter, nameof(SoftDeletableEntity.State));
        var notDeleted = Expression.NotEqual(state, Expression.Constant(RecordState.Deleted));
        return Expression.Lambda(notDeleted, parameter);
    }
}
