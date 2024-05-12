using ClickStitch.Api.Patterns.CreatePattern.Parsing.Types;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ClickStitch.Api.Patterns.CreatePattern;

public static class PatternThumbnailGenerator
{
    private static readonly DrawingOptions DrawingOptions = new()
    {
        GraphicsOptions = new GraphicsOptions
        {
            Antialias = false
        }
    };

    public static Stream Create(
        int width,
        int height,
        List<ParsePatternResponse.ThreadDetails> threads,
        List<ParsePatternResponse.StitchDetails> stitches,
        List<ParsePatternResponse.BackStitchDetails> backStitches)
    {
        var threadToColourMapping = threads.ToDictionary(x => x.Index, x => Rgba32.ParseHex(x.Colour));

        using var image = new Image<Rgba32>(width, height);

        foreach (var stitch in stitches)
        {
            image[stitch.X, stitch.Y] = threadToColourMapping[stitch.ThreadIndex];
        }

        image.Mutate(img =>
        {
            foreach (var backStitch in backStitches)
            {
                var colour = new Color(threadToColourMapping[backStitch.ThreadIndex]);

                img.DrawLine(DrawingOptions, Pens.Solid(colour), [
                    new(backStitch.StartX, backStitch.StartY),
                    new(backStitch.EndX, backStitch.EndY),
                ]);
            }
        });

        var stream = new MemoryStream();
        image.SaveAsPng(stream);
        stream.Position = 0;

        return stream;
    }
}