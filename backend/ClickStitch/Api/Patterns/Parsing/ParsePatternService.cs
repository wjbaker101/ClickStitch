using ClickStitch.Api.Patterns.Parsing.Parsers;
using ClickStitch.Api.Patterns.Parsing.Types;
using Inkwell.Client;
using Inkwell.Client.Types;

namespace ClickStitch.Api.Patterns.Parsing;

public interface IPatternParserService
{
    Result<ParsePatternResponse> Parse(ParsePatternParameters parameters);
}

public sealed class PatternParserService : IPatternParserService
{
    private readonly IInkwellClient _inkwell;

    public PatternParserService(IInkwellClient inkwell)
    {
        _inkwell = inkwell;
    }

    public Result<ParsePatternResponse> Parse(ParsePatternParameters parameters)
    {
        var isXml = parameters.RawContent.StartsWith("<?xml");

        var parser = GetParser(isXml);

        try
        {
            return parser.Parse(parameters);
        }
        catch (Exception e)
        {
            _inkwell.Log(new CreateLogRequest
            {
                LogLevel = InkwellLogLevel.Error,
                Message = e.Message,
                StackTrace = e.ToString(),
                JsonData = new
                {
                    Parameters = parameters
                }
            });

            return Result<ParsePatternResponse>.Failure("Unable to parse pattern schematic, please try again with a different format.");
        }
    }

    private static IPatternParser GetParser(bool isXml)
    {
        if (isXml)
            return new FlossCrossOxsPatternParser();

        return new FlossCrossFcJsonPatternParser();
    }
}