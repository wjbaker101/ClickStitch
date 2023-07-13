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

        static bool IgnoreCanvasThread(CreatePatternData.Thread x) => x.index != 0;

        return new ParsePatternResponse
        {
            Pattern = new ParsePatternResponse.PatternDetails
            {
                Width = data.canvas.width,
                Height = data.canvas.height,
                ThreadCount = data.palette.threads.Count - 1,
                StitchCount = data.canvas.stitches.Count
            },
            Threads = data.palette.threads
                .Where(IgnoreCanvasThread)
                .MapAll(x => new ParsePatternResponse.ThreadDetails
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