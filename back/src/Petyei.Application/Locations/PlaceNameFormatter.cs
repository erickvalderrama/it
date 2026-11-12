using System.Globalization;

namespace Petyei.Application.Locations;

/// <summary>
/// Turns official upper-case place names ("EL CARMEN DE BOLÍVAR") into display names
/// ("El Carmen de Bolívar"). The official name is kept for traceability.
/// </summary>
public static class PlaceNameFormatter
{
    private static readonly HashSet<string> LowerCaseWords = new(StringComparer.Ordinal)
    {
        "de", "del", "la", "las", "los", "el", "y", "e", "en",
    };

    private static readonly CultureInfo Spanish = CultureInfo.GetCultureInfo("es-CO");

    public static string ToDisplayName(string officialName)
    {
        ArgumentNullException.ThrowIfNull(officialName);
        var words = officialName.Trim().ToLower(Spanish).Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < words.Length; i++)
        {
            if (i > 0 && LowerCaseWords.Contains(words[i]))
            {
                continue;
            }

            words[i] = Capitalize(words[i]);
        }

        return string.Join(' ', words);
    }

    // Capitalizes each part of hyphenated or dotted words: "d.c." -> "D.C.", "san josé-del" -> "San José-Del".
    private static string Capitalize(string word)
    {
        var chars = word.ToCharArray();
        var startOfPart = true;
        for (var i = 0; i < chars.Length; i++)
        {
            if (startOfPart && char.IsLetter(chars[i]))
            {
                chars[i] = char.ToUpper(chars[i], Spanish);
                startOfPart = false;
            }
            else if (chars[i] is '-' or '.' or '(')
            {
                startOfPart = true;
            }
        }

        return new string(chars);
    }
}
