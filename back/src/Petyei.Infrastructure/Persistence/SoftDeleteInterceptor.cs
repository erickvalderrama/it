using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Petyei.Application.Common;
using Petyei.Domain.Common;

namespace Petyei.Infrastructure.Persistence;

/// <summary>Turns every delete of a business record into a logical deletion before saving.</summary>
internal sealed class SoftDeleteInterceptor(ICurrentActor actor, TimeProvider clock) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ConvertDeletes(eventData.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ConvertDeletes(eventData.Context);
        return ValueTask.FromResult(result);
    }

    private void ConvertDeletes(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var now = clock.GetUtcNow();
        foreach (var entry in context.ChangeTracker.Entries<SoftDeletableEntity>())
        {
            if (entry.State != EntityState.Deleted)
            {
                continue;
            }

            entry.State = EntityState.Modified;
            entry.Entity.MarkDeleted(actor.AccountId, now, reason: null);
        }
    }
}
