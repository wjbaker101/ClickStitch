namespace ClickStitch.Api.Patterns.CreatePattern.Parsing.Types;

public interface IPatternParser
{
    Result<ParsePatternResponse> Parse(ParsePatternParameters parameters);
}