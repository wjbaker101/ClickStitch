using ClickStitch.Api.Patterns.Parsing.Parsers;
using ClickStitch.Api.Patterns.Parsing.Types;

namespace ClickStitch.Api.Patterns.Parsing;

public interface IPatternParserService
{
    Result<ParsePatternResponse> Parse(ParsePatternParameters parameters);
}

public sealed class PatternParserService : IPatternParserService
{
    public PatternParserService()
    {
    }

    public Result<ParsePatternResponse> Parse(ParsePatternParameters parameters)
    {
        var isXml = parameters.RawContent.StartsWith("<?xml");

        var parser = GetParser(isXml);

        return parser.Parse(parameters);
    }

    private static IPatternParser GetParser(bool isXml)
    {
        if (isXml)
            return new FlossCrossOxsPatternParser();

        return new FlossCrossFcJsonPatternParser();
    }
}