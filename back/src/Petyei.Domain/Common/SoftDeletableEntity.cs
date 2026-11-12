namespace Petyei.Domain.Common;

/// <summary>
/// Base for business records. Records are never removed from the database: deleting one
/// changes its state and stores who deleted it, when, and why.
/// </summary>
public abstract class SoftDeletableEntity
{
    public RecordState State { get; private set; } = RecordState.Active;

    public DateTimeOffset? DeletedAt { get; private set; }

    public Guid? DeletedBy { get; private set; }

    public string? DeletionReason { get; private set; }

    public bool IsDeleted => State == RecordState.Deleted;

    public void MarkDeleted(Guid? actorId, DateTimeOffset at, string? reason)
    {
        if (IsDeleted)
        {
            return;
        }

        State = RecordState.Deleted;
        DeletedAt = at;
        DeletedBy = actorId;
        DeletionReason = reason;
    }

    public void Restore()
    {
        State = RecordState.Active;
        DeletedAt = null;
        DeletedBy = null;
        DeletionReason = null;
    }
}
