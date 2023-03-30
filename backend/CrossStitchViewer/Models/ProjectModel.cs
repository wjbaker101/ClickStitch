namespace CrossStitchViewer.Models;

public sealed class ProjectModel
{
    public required PatternModel Pattern { get; init; }
    public required DateTime PurchasedAt { get; init; }
}