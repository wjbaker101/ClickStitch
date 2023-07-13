using ClickStitch.Api.Patterns.Parsing.Types;
using ClickStitch.Api.Patterns.Types;
using Core.Extensions;
using Utf8Json;

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
        var data = JsonSerializer.Deserialize<CreatePatternData>(parameters.RawContent);

        var threadsWithoutCanvasThread = data.palette.threads
            .Where(x => x.index != 0)
            .ToList();

        return new ParsePatternResponse
        {
            Pattern = new ParsePatternResponse.PatternDetails
            {
                Width = data.canvas.width,
                Height = data.canvas.height,
                ThreadCount = threadsWithoutCanvasThread.Count,
                StitchCount = data.canvas.stitches.Count
            },
            Threads = threadsWithoutCanvasThread.MapAll(x => new ParsePatternResponse.ThreadDetails
            {
                Name = x.name,
                Description = x.description,
                Index = x.index,
                Colour = $"#{x.colour.ToLower()}"
            }),
            Stitches = data.canvas.stitches.ConvertAll(x => new ParsePatternResponse.StitchDetails
            {
                ThreadIndex = x.index,
                X = x.x,
                Y = x.y
            })
        };
    }
}