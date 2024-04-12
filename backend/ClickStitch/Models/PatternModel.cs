namespace ClickStitch.Models;

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
    public required string BannerImageUrl { get; init; }
    public required string? ExternalShopUrl { get; init; }
    public required string TitleSlug { get; init; }
    public required int AidaCount { get; init; }
    public required CreatorModel? Creator { get; init; }
    public required UserModel User { get; init; }
}

public sealed class PatternThreadModel
{
    public required int Index { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Colour { get; init; }
}