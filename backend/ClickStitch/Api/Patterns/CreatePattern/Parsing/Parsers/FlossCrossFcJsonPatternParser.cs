using ClickStitch.Api.Patterns.CreatePattern.Parsing;
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

        var threadIndexMapping = image.crossIndexes
            .Select((value, index) => new { value, index })
            .ToDictionary(x => x.value.fi, x => x.index);

        var stitches = new List<ParsePatternResponse.StitchDetails>();
        var posX = 0;
        var posY = 0;

        foreach (var threadIndex in layer.cross)
        {
            if (threadIndex != -1)
            {
                stitches.Add(new ParsePatternResponse.StitchDetails
                {
                    ThreadIndex = threadIndex + 1,
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
                ThreadCount = threadIndexMapping.Count,
                StitchCount = layer.cross.Count
            },
            Threads = threadIndexMapping.Select(x => image.flossIndexes[x.Key]).MapAll((x, index) => new ParsePatternResponse.ThreadDetails
            {
                Name = $"{x.sys} {x.id}",
                Description = x.name,
                Index = index + 1,
                Colour = ParsingHelper.RgbToHex(x.rgb[0], x.rgb[1], x.rgb[2])
            }),
            Stitches = stitches
        };
    }
}