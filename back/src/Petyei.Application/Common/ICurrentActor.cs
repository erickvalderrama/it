namespace Petyei.Application.Common;

/// <summary>Identifies the authenticated account performing the current operation, if any.</summary>
public interface ICurrentActor
{
    Guid? AccountId { get; }
}
