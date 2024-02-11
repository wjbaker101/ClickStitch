namespace ClickStitch.Models;

public sealed class ProjectModel
{
    public required PatternModel Pattern { get; init; }
    public required Guid Reference { get; init; }
    public required DateTime PurchasedAt { get; init; }
    public required int? PausePositionX { get; init; }
    public required int? PausePositionY { get; init; }
}