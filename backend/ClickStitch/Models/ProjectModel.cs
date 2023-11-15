namespace ClickStitch.Models;

public sealed class ProjectModel
{
    public required PatternModel Pattern { get; init; }
    public required DateTime PurchasedAt { get; init; }
    public required int? LastPositionX { get; init; }
    public required int? LastPositionY { get; init; }
}