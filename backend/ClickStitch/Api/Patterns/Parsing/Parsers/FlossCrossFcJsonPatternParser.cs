using ClickStitch.Api.Patterns.Parsing.Types;
using Utf8Json;

namespace ClickStitch.Api.Patterns.Parsing.Parsers;

public sealed class FlossCrossFcJsonPatternParser : IPatternParser
{
    // ReSharper disable InconsistentNaming
    #pragma warning disable IDE1006
    public sealed class PatternFormat
    {
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

        return null!;
    }
}