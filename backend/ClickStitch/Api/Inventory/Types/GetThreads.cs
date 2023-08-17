namespace ClickStitch.Api.Inventory.Types;

public sealed class GetThreadsResponse
{
    public required List<ThreadModel> Threads { get; init; }
}