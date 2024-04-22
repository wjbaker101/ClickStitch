using ClickStitch.Api.Patterns.CreatePattern.Parsing;
using ClickStitch.Api.Patterns.CreatePattern.Parsing.Types;
using DotNetLibs.Core.Types;

namespace TestHelpers.Fakes;

public sealed class FakePatternParserService : IPatternParserService
{
    public Result<ParsePatternResponse> Response { get; set; } = new ParsePatternResponse
    {
        Pattern = null!,
        Threads = null!,
        Stitches = null!
    };

    public ParsePatternParameters? ActualParameters { get; private set; }

    public Result<ParsePatternResponse> Parse(ParsePatternParameters parameters)
    {
        ActualParameters = parameters;

        return Response;
    }
}