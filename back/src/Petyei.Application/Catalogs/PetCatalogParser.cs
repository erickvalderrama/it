using Petyei.Application.Common;
using Petyei.Domain.Common;

namespace Petyei.Application.Catalogs;

/// <summary>Display names of a catalog record, by locale.</summary>
public sealed record LocalizedNames(string English, string Spanish);

public sealed record SpeciesRow(string Code, int SortOrder, LocalizedNames Names);

public sealed record BreedRow(string SpeciesCode, string Code, LocalizedNames Names);

public sealed record PetCatalogData(IReadOnlyList<SpeciesRow> Species, IReadOnlyList<BreedRow> Breeds);

/// <summary>Reads and validates the bilingual species and breeds file.</summary>
public static class PetCatalogParser
{
    private static readonly string[] Columns = ["kind", "species_code", "breed_code", "sort_order", "name_en", "name_es"];

    public static (PetCatalogData? Data, IReadOnlyList<string> Problems) Parse(string csv)
    {
        var records = CsvParser.ReadRecords(csv);
        var problems = new List<string>();
        if (records.Count == 0)
        {
            return (null, ["The species file has no rows."]);
        }

        problems.AddRange(Columns.Where(c => !records[0].ContainsKey(c)).Select(c => $"The species file is missing the column '{c}'."));
        if (problems.Count > 0)
        {
            return (null, problems);
        }

        var species = new List<SpeciesRow>();
        var breeds = new List<BreedRow>();
        for (var line = 0; line < records.Count; line++)
        {
            var record = records[line];
            var names = new LocalizedNames(record["name_en"].Trim(), record["name_es"].Trim());
            var where = $"Row {line + 2}";
            if (names.English.Length == 0 || names.Spanish.Length == 0)
            {
                problems.Add($"{where} needs English and Spanish names.");
            }

            if (!IsCode(record["species_code"]))
            {
                problems.Add($"{where} has an invalid species code '{record["species_code"]}'.");
                continue;
            }

            switch (record["kind"])
            {
                case "species" when int.TryParse(record["sort_order"], out var sortOrder):
                    species.Add(new SpeciesRow(record["species_code"], sortOrder, names));
                    break;
                case "species":
                    problems.Add($"{where} needs a numeric sort order.");
                    break;
                case "breed" when IsCode(record["breed_code"]):
                    breeds.Add(new BreedRow(record["species_code"], record["breed_code"], names));
                    break;
                case "breed":
                    problems.Add($"{where} has an invalid breed code '{record["breed_code"]}'.");
                    break;
                default:
                    problems.Add($"{where} has an unknown kind '{record["kind"]}'.");
                    break;
            }
        }

        var speciesCodes = species.Select(s => s.Code).ToHashSet(StringComparer.Ordinal);
        problems.AddRange(species.GroupBy(s => s.Code).Where(g => g.Count() > 1).Select(g => $"Duplicate species {g.Key}."));
        problems.AddRange(breeds.GroupBy(b => (b.SpeciesCode, b.Code)).Where(g => g.Count() > 1)
            .Select(g => $"Duplicate breed {g.Key.SpeciesCode}.{g.Key.Code}."));
        problems.AddRange(breeds.Where(b => !speciesCodes.Contains(b.SpeciesCode))
            .Select(b => $"Breed {b.Code} references missing species {b.SpeciesCode}."));

        return problems.Count > 0 ? (null, problems) : (new PetCatalogData(species, breeds), problems);
    }

    private static bool IsCode(string value)
    {
        try
        {
            CatalogCode.Require(value);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}
