using ClickStitch.Api.Patterns.CreatePattern.Parsing.Types;
using DotNetLibs.Core.Extensions;
using Utf8Json;

namespace ClickStitch.Api.Patterns.CreatePattern.Parsing.Parsers;

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
            public required List<CrossIndex> crossIndexes { get; init; }
            public required List<Layer> layers { get; init; }
        }

        public sealed class FlossIndex
        {
            public required string id { get; init; }
            public required string name { get; init; }
            public required int[] rgb { get; init; }
            public required string sys { get; init; }
        }

        public sealed class CrossIndex
        {
            public required int fi { get; init; }
        }

        public sealed class Layer
        {
            public required List<int> cross { get; init; }
            public required List<BackStitch> backstitch { get; init; }
        }

        public sealed class BackStitch
        {
            public required int c { get; init; }
            public required int[][] p { get; init; }
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

        var crossToFlossIndexMapping = image.crossIndexes
            .Select((value, index) => new { value, index })
            .ToDictionary(x => x.index, x => x.value.fi);

        var flossToThreadIndexMapping = crossToFlossIndexMapping
            .Select(x => x.Value)
            .Concat(layer.backstitch.Select(x => x.c))
            .Distinct()
            .Select((x, index) => new
            {
                value = x,
                index = index + 1
            })
            .ToDictionary(x => x.value, x => x.index);

        var stitches = new List<ParsePatternResponse.StitchDetails>();
        var posX = 0;
        var posY = 0;

        foreach (var threadIndex in layer.cross)
        {
            if (threadIndex != -1)
            {
                stitches.Add(new ParsePatternResponse.StitchDetails
                {
                    ThreadIndex = flossToThreadIndexMapping[crossToFlossIndexMapping[threadIndex]],
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

        var backStitches = layer.backstitch.ConvertAll(x => new ParsePatternResponse.BackStitchDetails
        {
            ThreadIndex = flossToThreadIndexMapping[x.c],
            StartX = x.p[0][0],
            StartY = x.p[0][1],
            EndX = x.p[0][2],
            EndY = x.p[0][3]
        });

        return new ParsePatternResponse
        {
            Pattern = new ParsePatternResponse.PatternDetails
            {
                Width = image.width,
                Height = image.height,
                ThreadCount = flossToThreadIndexMapping.Count,
                StitchCount = stitches.Count + backStitches.Count
            },
            Threads = flossToThreadIndexMapping.MapAll(x => MapThread(image.flossIndexes[x.Key], x.Value)),
            Stitches = stitches,
            BackStitches = backStitches
        };
    }

    private static ParsePatternResponse.ThreadDetails MapThread(PatternFormat.FlossIndex floss, int index) => new()
    {
        Name = $"{floss.sys} {floss.id}",
        Description = floss.name,
        Index = index,
        Colour = ParsingHelper.RgbToHex(floss.rgb[0], floss.rgb[1], floss.rgb[2])
    };
}