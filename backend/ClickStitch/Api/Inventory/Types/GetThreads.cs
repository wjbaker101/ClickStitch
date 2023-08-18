namespace ClickStitch.Api.Inventory.Types;

public sealed class GetThreadsResponse
{
    public required List<InventoryThread> Threads { get; init; }

    public sealed class InventoryThread
    {
        public required ThreadModel Thread { get; init; }
        public required int Count { get; init; }
    }
}