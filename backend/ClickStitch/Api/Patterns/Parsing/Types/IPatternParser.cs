namespace ClickStitch.Api.Patterns.Parsing.Types;

public interface IPatternParser
{
    Result<ParsePatternResponse> Parse(ParsePatternParameters parameters);
}