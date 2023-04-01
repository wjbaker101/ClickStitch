namespace CrossStitchViewer.Models;

public sealed class PatternModel
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Title { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required decimal Price { get; init; }
    public required string? ThumbnailUrl { get; init; }
    public required int ThreadCount { get; init; }
    public required int StitchCount { get; init; }
}