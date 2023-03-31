namespace CrossStitchViewer.Api.Patterns;

public sealed class PatternDataDto
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