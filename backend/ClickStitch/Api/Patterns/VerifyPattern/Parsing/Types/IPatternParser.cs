namespace ClickStitch.Api.Patterns.VerifyPattern.Parsing.Types;

public interface IPatternParser
{
    Result<ParsePatternResponse> Parse(ParsePatternParameters parameters);
}