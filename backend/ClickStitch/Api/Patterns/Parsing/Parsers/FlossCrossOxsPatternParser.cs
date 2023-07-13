using ClickStitch.Api.Patterns.Parsing.Types;
using System.Text;
using System.Xml.Serialization;

namespace ClickStitch.Api.Patterns.Parsing.Parsers;

public sealed class FlossCrossOxsPatternParser : IPatternParser
{
    [XmlRoot("chart")]
    public sealed class PatternFormat
    {
        [XmlElement("properties")]
        public required PropertiesDetails Properties { get; init; }

        [XmlArray("palette")]
        [XmlArrayItem("palette_item")]
        public required List<PaletteItem> Palette { get; init; }

        [XmlArray("fullstitches")]
        [XmlArrayItem("stitch")]
        public required List<Stitch> Stitches { get; init; }

        public sealed class PropertiesDetails
        {
            [XmlAttribute("chartwidth")]
            public required int ChartWidth { get; init; }

            [XmlAttribute("chartheight")]
            public required int ChartHeight { get; init; }
        }

        public sealed class PaletteItem
        {
            [XmlAttribute("index")]
            public required int Index { get; init; }

            [XmlAttribute("number")]
            public required string Number { get; init; }

            [XmlAttribute("name")]
            public required string Name { get; init; }

            [XmlAttribute("color")]
            public required string Colour { get; init; }
        }

        public sealed class Stitch
        {
            [XmlAttribute("x")]
            public required int X { get; init; }

            [XmlAttribute("y")]
            public required int Y { get; init; }

            [XmlAttribute("palindex")]
            public required int ThreadIndex { get; init; }
        }
    }

    public Result<ParsePatternResponse> Parse(ParsePatternParameters parameters)
    {
        var serialiser = new XmlSerializer(typeof(PatternFormat));

        PatternFormat? data;
        using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(parameters.RawContent))))
        {
            data = serialiser.Deserialize(reader) as PatternFormat;
        }

        if (data == null)
            return Result<ParsePatternResponse>.Failure("Unable to parse pattern schematic.");

        var threadsWithoutCanvasThread = data.Palette
            .Where(x => x.Index != 0)
            .ToList();

        return new ParsePatternResponse
        {
            Pattern = new ParsePatternResponse.PatternDetails
            {
                Width = data.Properties.ChartWidth,
                Height = data.Properties.ChartHeight,
                ThreadCount = threadsWithoutCanvasThread.Count,
                StitchCount = data.Stitches.Count
            },
            Threads = threadsWithoutCanvasThread.ConvertAll(x => new ParsePatternResponse.ThreadDetails
            {
                Name = x.Number,
                Description = x.Name,
                Index = x.Index,
                Colour = $"#{x.Colour}"
            }),
            Stitches = data.Stitches.ConvertAll(x => new ParsePatternResponse.StitchDetails
            {
                ThreadIndex = x.ThreadIndex,
                X = x.X,
                Y = x.Y
            })
        };
    }
}