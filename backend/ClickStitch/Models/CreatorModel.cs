namespace ClickStitch.Models;

public sealed class CreatorModel
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Name { get; init; }
    public required string StoreUrl { get; init; }
}