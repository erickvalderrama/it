using Petyei.Application.Common;
using Petyei.Domain.Locations;

namespace Petyei.Application.Catalogs;

public sealed record DepartmentRow(string Code, string Name);

public sealed record LocalityRow(string DepartmentCode, string Code, string Name, string LocalityType);

public sealed record LocationCatalogData(
    CountryCode Country,
    IReadOnlyList<DepartmentRow> Departments,
    IReadOnlyList<LocalityRow> Localities,
    IReadOnlyDictionary<string, int> LocalitiesByType);

/// <summary>Reads and validates the DIVIPOLA department and locality files before any database change.</summary>
public static class LocationCatalogParser
{
    public static (LocationCatalogData? Data, IReadOnlyList<string> Problems) Parse(string departmentsCsv, string localitiesCsv)
    {
        var problems = new List<string>();
        var departmentRecords = CsvParser.ReadRecords(departmentsCsv);
        var localityRecords = CsvParser.ReadRecords(localitiesCsv);

        RequireColumns(departmentRecords, ["country_code", "department_code", "department_name"], "departments", problems);
        RequireColumns(localityRecords, ["country_code", "department_code", "locality_code", "locality_name", "locality_type"], "localities", problems);
        if (problems.Count > 0)
        {
            return (null, problems);
        }

        var countries = departmentRecords.Select(r => r["country_code"]).Concat(localityRecords.Select(r => r["country_code"])).Distinct().ToList();
        if (countries.Count != 1 || !CountryCode.TryCreate(countries[0], out var country))
        {
            return (null, [$"Files must contain exactly one valid country code; found: {string.Join(", ", countries)}."]);
        }

        var departments = departmentRecords.Select(r => new DepartmentRow(r["department_code"].Trim(), r["department_name"].Trim())).ToList();
        var localities = localityRecords
            .Select(r => new LocalityRow(r["department_code"].Trim(), r["locality_code"].Trim(), r["locality_name"].Trim(), r["locality_type"].Trim()))
            .ToList();

        ReportDuplicates(departments.Select(d => d.Code), "department", problems);
        ReportDuplicates(localities.Select(l => l.Code), "locality", problems);

        var departmentCodes = departments.Select(d => d.Code).ToHashSet(StringComparer.Ordinal);
        foreach (var department in departments.Where(d => d.Code.Length == 0 || d.Name.Length == 0))
        {
            problems.Add($"Department '{department.Code}' needs a code and a name.");
        }

        foreach (var locality in localities)
        {
            if (!departmentCodes.Contains(locality.DepartmentCode))
            {
                problems.Add($"Locality {locality.Code} references missing department {locality.DepartmentCode}.");
            }

            if (!locality.Code.StartsWith(locality.DepartmentCode, StringComparison.Ordinal))
            {
                problems.Add($"Locality {locality.Code} does not start with its department code {locality.DepartmentCode}.");
            }

            if (locality.Name.Length == 0 || locality.LocalityType.Length == 0)
            {
                problems.Add($"Locality {locality.Code} needs a name and a type.");
            }
        }

        var byType = localities.GroupBy(l => l.LocalityType, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.Ordinal);
        return problems.Count > 0 ? (null, problems) : (new LocationCatalogData(country, departments, localities, byType), problems);
    }

    private static void RequireColumns(
        IReadOnlyList<IReadOnlyDictionary<string, string>> records,
        string[] columns,
        string file,
        List<string> problems)
    {
        if (records.Count == 0)
        {
            problems.Add($"The {file} file has no rows.");
            return;
        }

        foreach (var column in columns.Where(column => !records[0].ContainsKey(column)))
        {
            problems.Add($"The {file} file is missing the column '{column}'.");
        }
    }

    private static void ReportDuplicates(IEnumerable<string> codes, string kind, List<string> problems)
    {
        foreach (var code in codes.GroupBy(c => c, StringComparer.Ordinal).Where(g => g.Count() > 1).Select(g => g.Key))
        {
            problems.Add($"Duplicate {kind} code {code}.");
        }
    }
}
