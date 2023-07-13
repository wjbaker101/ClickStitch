using Data.Records;

namespace ClickStitch.Api.Patterns.Parsing.Types;

public sealed class ParsePatternParameters
{
    public required string RawContent { get; init; }
    public required string Title { get; init; }
    public required string TitleSlug { get; init; }
    public required decimal Price { get; init; }
    public required int AidaCount { get; init; }
    public required string ThumbnailUrl { get; init; }
    public required string BannerImageUrl { get; init; }
    public required CreatorRecord Creator { get; init; }
    public required string ExternalShopUrl { get; init; }
}

public sealed class ParsePatternResponse
{
    public required PatternRecord Pattern { get; init; }
    public required List<PatternThreadRecord> Threads { get; init; }
    public required List<PatternThreadStitchRecord> Stitches { get; init; }
}