using ClickStitch.Api.Patterns.Parsing.Types;
using DotNetLibs.Core.Extensions;
using Utf8Json;

namespace ClickStitch.Api.Patterns.Parsing.Parsers;

public sealed class FlossCrossFcJsonPatternParser : IPatternParser
{
    // ReSharper disable InconsistentNaming
    #pragma warning disable IDE1006
    public sealed class PatternFormat
    {
        public required Model model { get; init; }

        public sealed class Model
        {
            public required List<Image> images { get; init; }
        }

        public sealed class Image
        {
            public required int width { get; init; }
            public required int height { get; init; }
            public required List<FlossIndex> flossIndexes { get; init; }
            public required List<Layer> layers { get; init; }
        }

        public sealed class FlossIndex
        {
            public required string id { get; init; }
            public required string name { get; init; }
            public required int[] rgb { get; init; }
            public required string sys { get; init; }
        }

        public sealed class Layer
        {
            public required List<int> cross { get; init; }
        }
    }
    #pragma warning restore IDE1006
    // ReSharper restore InconsistentNaming

    public Result<ParsePatternResponse> Parse(ParsePatternParameters parameters)
    {
        var data = JsonSerializer.Deserialize<PatternFormat>(parameters.RawContent);
        if (data == null)
            return Result<ParsePatternResponse>.Failure("Unable to parse pattern schematic.");

        var image = data.model.images[0];
        var layer = image.layers[0];

        var stitches = new List<ParsePatternResponse.StitchDetails>();
        var posX = 0;
        var posY = 0;

        foreach (var threadIndex in layer.cross)
        {
            if (threadIndex != -1)
            {
                stitches.Add(new ParsePatternResponse.StitchDetails
                {
                    ThreadIndex = threadIndex,
                    X = posX,
                    Y = posY
                });
            }

            posX++;
            if (posX % image.width == 0)
            {
                posY++;
                posX = 0;
            }
        }

        return new ParsePatternResponse
        {
            Pattern = new ParsePatternResponse.PatternDetails
            {
                Width = image.width,
                Height = image.height,
                ThreadCount = image.flossIndexes.Count,
                StitchCount = layer.cross.Count
            },
            Threads = image.flossIndexes.MapAll((x, index) => new ParsePatternResponse.ThreadDetails
            {
                Name = $"{x.sys} {x.id}",
                Description = x.name,
                Index = index,
                Colour = ParsingHelper.RgbToHex(x.rgb[0], x.rgb[1], x.rgb[2])
            }),
            Stitches = stitches
        };
    }
}