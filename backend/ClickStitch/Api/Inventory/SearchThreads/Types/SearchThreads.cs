namespace ClickStitch.Api.Inventory.SearchThreads.Types;

public sealed class SearchThreadsParameters
{
    public required string? SearchTerm { get; init; }
    public required string? Brand { get; init; }
}

public sealed class SearchThreadsResponse
{
    public required List<InventoryThread> InventoryThreads { get; init; }
    public required List<ThreadModel> AvailableThreads { get; init; }

    public sealed class InventoryThread
    {
        public required ThreadModel Thread { get; init; }
        public required int Count { get; init; }
    }
}