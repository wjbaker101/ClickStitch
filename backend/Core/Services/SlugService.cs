using DotNetLibs.Core.Types;
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

    public static Result<string> Generate(string text)
    {
        var mutated = text;

        mutated = mutated.ToLower().Trim();
        mutated = AlphanumericRegex().Replace(mutated, "");
        mutated = TrimSpacesRegex().Replace(mutated, " ");
        mutated = TrimDashesRegex().Replace(mutated, " ");
        mutated = mutated.Replace(" ", "-");

        if (mutated.Length == 0)
            return Result<string>.Failure("Generated slug cannot be empty.");

        return mutated;
    }
}