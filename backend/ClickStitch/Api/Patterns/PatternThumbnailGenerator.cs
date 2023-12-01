using ClickStitch.Api.Patterns.Parsing.Types;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ClickStitch.Api.Patterns;

public static class PatternThumbnailGenerator
{
    public static Stream Create(int width, int height, List<ParsePatternResponse.ThreadDetails> threads, List<ParsePatternResponse.StitchDetails> stitches)
    {
        var threadLookup = threads.ToDictionary(x => x.Index);

        var image = new Image<Rgba32>(width, height);

        foreach (var stitch in stitches)
        {
            image[stitch.X, stitch.Y] = Rgba32.ParseHex(threadLookup[stitch.ThreadIndex].Colour);
        }

        var stream = new MemoryStream();
        image.SaveAsPng(stream);
        stream.Position = 0;

        return stream;
    }
}