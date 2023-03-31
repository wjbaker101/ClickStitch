namespace CrossStitchViewer.Models;

public sealed class StitchModel
{
    public required int ThreadIndex { get; init; }
    public required int X { get; init; }
    public required int Y { get; init; }
}