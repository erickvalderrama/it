using Petyei.Domain.Common;

namespace Petyei.Domain.Tests.Common;

public sealed class SoftDeletableEntityTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 12, 15, 0, 0, TimeSpan.Zero);

    [Fact]
    public void MarkDeleted_RecordsActorTimeAndReason()
    {
        var record = new Sample();
        var actor = Guid.NewGuid();

        record.MarkDeleted(actor, Now, "Duplicate listing");

        Assert.True(record.IsDeleted);
        Assert.Equal(Now, record.DeletedAt);
        Assert.Equal(actor, record.DeletedBy);
        Assert.Equal("Duplicate listing", record.DeletionReason);
    }

    [Fact]
    public void MarkDeleted_KeepsTheFirstDeletionWhenRepeated()
    {
        var record = new Sample();
        var firstActor = Guid.NewGuid();

        record.MarkDeleted(firstActor, Now, "first");
        record.MarkDeleted(Guid.NewGuid(), Now.AddHours(1), "second");

        Assert.Equal(firstActor, record.DeletedBy);
        Assert.Equal(Now, record.DeletedAt);
    }

    [Fact]
    public void Restore_ClearsDeletionData()
    {
        var record = new Sample();
        record.MarkDeleted(Guid.NewGuid(), Now, "reason");

        record.Restore();

        Assert.Equal(RecordState.Active, record.State);
        Assert.Null(record.DeletedAt);
        Assert.Null(record.DeletedBy);
        Assert.Null(record.DeletionReason);
    }

    private sealed class Sample : SoftDeletableEntity;
}
