using System.Text;

namespace Petyei.Application.Common;

/// <summary>Minimal RFC 4180 reader: quoted fields, escaped quotes, and commas inside quotes.</summary>
public static class CsvParser
{
    public static IReadOnlyList<IReadOnlyDictionary<string, string>> ReadRecords(string content)
    {
        ArgumentNullException.ThrowIfNull(content);
        var rows = ReadRows(content.TrimStart('﻿'));
        if (rows.Count == 0)
        {
            return [];
        }

        var header = rows[0];
        var records = new List<IReadOnlyDictionary<string, string>>(rows.Count - 1);
        foreach (var row in rows.Skip(1))
        {
            var record = new Dictionary<string, string>(StringComparer.Ordinal);
            for (var column = 0; column < header.Count; column++)
            {
                record[header[column]] = column < row.Count ? row[column] : string.Empty;
            }

            records.Add(record);
        }

        return records;
    }

    private static List<List<string>> ReadRows(string content)
    {
        var rows = new List<List<string>>();
        var row = new List<string>();
        var field = new StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < content.Length; i++)
        {
            var c = content[i];
            if (inQuotes)
            {
                if (c == '"' && i + 1 < content.Length && content[i + 1] == '"')
                {
                    field.Append('"');
                    i++;
                }
                else if (c == '"')
                {
                    inQuotes = false;
                }
                else
                {
                    field.Append(c);
                }

                continue;
            }

            switch (c)
            {
                case '"':
                    inQuotes = true;
                    break;
                case ',':
                    row.Add(field.ToString());
                    field.Clear();
                    break;
                case '\r':
                    break;
                case '\n':
                    row.Add(field.ToString());
                    field.Clear();
                    AddRow(rows, row);
                    row = [];
                    break;
                default:
                    field.Append(c);
                    break;
            }
        }

        row.Add(field.ToString());
        AddRow(rows, row);
        return rows;
    }

    private static void AddRow(List<List<string>> rows, List<string> row)
    {
        if (row.Count > 1 || row[0].Length > 0)
        {
            rows.Add(row);
        }
    }
}
