using ClickStitch.Api.Patterns.Parsing.Types;
using Core.Extensions;
using Utf8Json;

namespace ClickStitch.Api.Patterns.Parsing.Parsers;

public sealed class DefaultPatternParser : IPatternParser
{
    // ReSharper disable InconsistentNaming
    #pragma warning disable IDE1006
    public sealed class PatternFormat
    {
        public required Palette palette { get; init; }
        public required Canvas canvas { get; init; }

        public sealed class Palette
        {
            public required List<Thread> threads { get; init; }
        }

        public sealed class Thread
        {
            public required int index { get; init; }
            public required string name { get; init; }
            public required string description { get; init; }
            public required string colour { get; init; }
        }

        public sealed class Canvas
        {
            public required int width { get; init; }
            public required int height { get; init; }
            public required List<Stitch> stitches { get; init; }
        }

        public sealed class Stitch
        {
            public required int x { get; init; }
            public required int y { get; init; }
            public required int index { get; init; }
        }
    }
    #pragma warning restore IDE1006
    // ReSharper restore InconsistentNaming

    public Result<ParsePatternResponse> Parse(ParsePatternParameters parameters)
    {
        var data = JsonSerializer.Deserialize<PatternFormat>(parameters.RawContent);
        if (data == null)
            return Result<ParsePatternResponse>.Failure("Unable to parse pattern schematic.");

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