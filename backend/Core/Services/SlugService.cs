using System.Text.RegularExpressions;

namespace Core.Services;

public static partial class SlugService
{
    [GeneratedRegex(@"[^a-z0-9-\s]")]
    private static partial Regex AlphanumericRegex();

    [GeneratedRegex("\\s+")]
    private static partial Regex TrimSpacesRegex();

    [GeneratedRegex("-+")]
    private static partial Regex TrimDashesRegex();

    public static string Generate(string text)
    {
        var mutated = text;

        mutated = AlphanumericRegex().Replace(mutated.ToLower().Trim(), "");
        mutated = TrimSpacesRegex().Replace(mutated, " ");
        mutated = TrimDashesRegex().Replace(mutated, " ");
        mutated = mutated.Replace(" ", "-");

        return mutated;
    }
}