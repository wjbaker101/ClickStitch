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
    public required PatternDetails Pattern { get; init; }
    public required List<ThreadDetails> Threads { get; init; }
    public required List<StitchDetails> Stitches { get; init; }

    public sealed class PatternDetails
    {
        public required int Width { get; init; }
        public required int Height { get; init; }
        public required int ThreadCount { get; init; }
        public required int StitchCount { get; init; }
    }

    public sealed class ThreadDetails
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required int Index { get; init; }
        public required string Colour { get; init; }
    }

    public sealed class StitchDetails
    {
        public required int ThreadIndex { get; init; }
        public required int X { get; init; }
        public required int Y { get; init; }
    }
}