namespace CrossStitchViewer.Models;

public sealed class PaletteModel
{
    public required List<ThreadModel> Threads { get; init; }
}

public sealed class ThreadModel
{
    public required int Index { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Colour { get; init; }
}