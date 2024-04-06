using ClickStitch.Api.Patterns.VerifyPattern.Parsing;
using ClickStitch.Api.Patterns.VerifyPattern.Parsing.Types;
using ClickStitch.Api.Patterns.VerifyPattern.Types;

namespace ClickStitch.Api.Patterns.VerifyPattern;

public interface IVerifyPatternService
{
    Result<VerifyPatternResponse> VerifyPattern(string patternData, CancellationToken cancellationToken);
}

public sealed class VerifyPatternService : IVerifyPatternService
{
    private readonly IPatternParserService _patternParserService;

    public VerifyPatternService(IPatternParserService patternParserService)
    {
        _patternParserService = patternParserService;
    }

    public Result<VerifyPatternResponse> VerifyPattern(string patternData, CancellationToken cancellationToken)
    {
        var parseResult = _patternParserService.Parse(new ParsePatternParameters
        {
            RawContent = patternData
        });
        if (parseResult.IsFailure)
            return Result<VerifyPatternResponse>.FromFailure(parseResult);

        return new VerifyPatternResponse();
    }
}