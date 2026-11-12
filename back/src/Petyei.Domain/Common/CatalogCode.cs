using System.Text.RegularExpressions;

namespace Petyei.Domain.Common;

/// <summary>Validates stable catalog keys: lower-case ASCII words separated by underscores.</summary>
public static partial class CatalogCode
{
    public const int MaxLength = 64;

    public static string Require(string? code)
    {
        if (code is null || code.Length > MaxLength || !Pattern().IsMatch(code))
        {
            throw new ArgumentException("Catalog codes use lower-case letters, digits, and underscores.", nameof(code));
        }

        return code;
    }

    [GeneratedRegex("^[a-z][a-z0-9]*(_[a-z0-9]+)*$")]
    private static partial Regex Pattern();
}
