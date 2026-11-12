using Petyei.Domain.Common;

namespace Petyei.Domain.Locations;

/// <summary>Country where Petyei can operate. Names are shown through catalog translations.</summary>
public sealed class Country : SoftDeletableEntity
{
    private Country()
    {
    }

    public string Code { get; private set; } = string.Empty;

    /// <summary>Whether people can register and publish in this country.</summary>
    public bool IsEnabled { get; private set; }

    public static Country Create(CountryCode code, bool isEnabled) => new() { Code = code.Value, IsEnabled = isEnabled };

    public void SetEnabled(bool isEnabled) => IsEnabled = isEnabled;
}
