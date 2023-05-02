namespace ClickStitch.Api.Projects.Types;

public sealed class GetAnalyticsResponse
{
    public required string Title { get; init; }
    public required string? ThumbnailUrl { get; init; }
    public required DateTime PurchasedAt { get; init; }
    public required int TotalStitches { get; init; }
    public required int CompletedStitches { get; init; }
    public required int RemainingStitches { get; init; }
}