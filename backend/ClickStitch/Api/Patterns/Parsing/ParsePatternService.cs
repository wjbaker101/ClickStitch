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
        return new DefaultPatternParser().Parse(parameters);
    }
}