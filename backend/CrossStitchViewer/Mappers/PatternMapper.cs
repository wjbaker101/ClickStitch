using CrossStitchViewer.Models;
using Data.Records;

namespace CrossStitchViewer.Mappers;

public static class PatternMapper
{
    public static PatternModel Map(PatternRecord pattern) => new()
    {
        Reference = pattern.Reference,
        CreatedAt = pattern.CreatedAt,
        Title = pattern.Title,
        Width = pattern.Width,
        Height = pattern.Height,
        Price = pattern.Price,
        ThumbnailUrl = pattern.ThumbnailUrl
    };

    public static StitchModel MapStitch(PatternStitchRecord stitch) => new()
    {
        ThreadIndex = stitch.ThreadIndex,
        X = stitch.X,
        Y = stitch.Y
    };

    public static ThreadModel MapThread(PatternThreadRecord thread) => new()
    {
        Index = thread.Index,
        Name = thread.Name,
        Description = thread.Description,
        Colour = thread.Colour
    };
}