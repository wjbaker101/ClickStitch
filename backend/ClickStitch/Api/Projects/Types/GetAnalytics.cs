namespace ClickStitch.Api.Projects.Types;

public sealed class GetAnalyticsResponse
{
    public required DateTime PurchasedAt { get; init; }
    public required int TotalStitches { get; init; }
    public required int CompletedStitches { get; init; }
    public required int RemainingStitches { get; init; }
}