namespace ClickStitch.Api.Patterns.Types;

public sealed class CreatePatternRequest
{
    public required string Title { get; init; }
    public required string ThumbnailFileName { get; init; }
    public required decimal Price { get; init; }
    public required int AidaCount { get; init; }
}

public sealed class CreatePatternData
{
    public required PaletteDetails palette { get; init; }
    public required CanvasDetails canvas { get; init; }

    public sealed class PaletteDetails
    {
        public required List<Thread> threads { get; init; }
    }

    public sealed class Thread
    {
        public required int index { get; init; }
        public required string name { get; init; }
        public required string description { get; init; }
        public required string colour { get; init; }
    }

    public sealed class CanvasDetails
    {
        public required int width { get; init; }
        public required int height { get; init; }
        public required List<Stitch> stitches { get; init; }
    }

    public sealed class Stitch
    {
        public required int x { get; init; }
        public required int y { get; init; }
        public required int index { get; init; }
    }
}