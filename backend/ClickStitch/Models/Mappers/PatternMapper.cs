using Data.Records;

namespace ClickStitch.Models.Mappers;

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
        ThumbnailUrl = pattern.ThumbnailUrl,
        ThreadCount = pattern.ThreadCount,
        StitchCount = pattern.StitchCount
    };

    public static StitchModel MapStitch(UserPatternStitchRecord stitch) => new()
    {
        Reference = stitch.Reference,
        ThreadIndex = stitch.PatternStitch.ThreadIndex,
        X = stitch.PatternStitch.X,
        Y = stitch.PatternStitch.Y,
        StitchedAt = stitch.StitchedAt
    };

    public static ThreadModel MapThread(PatternThreadRecord thread) => new()
    {
        Index = thread.Index,
        Name = thread.Name,
        Description = thread.Description,
        Colour = thread.Colour
    };
}