namespace Petyei.Domain.Common;

/// <summary>Lifecycle state shared by every record that supports logical deletion.</summary>
public enum RecordState
{
    Active = 1,
    Deleted = 2,
}
