namespace Petyei.Infrastructure.Locations;

public sealed class CountryCatalogOptions
{
    public const string SectionName = "Countries";

    public IList<string> Enabled { get; } = [];
}
