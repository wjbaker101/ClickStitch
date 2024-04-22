namespace ClickStitch.Api.Patterns.CreatePattern.Parsing;

public static class ParsingHelper
{
    public static string RgbToHex(int r, int g, int b)
    {
        return $"#{r:X2}{g:X2}{b:X2}";
    }
}