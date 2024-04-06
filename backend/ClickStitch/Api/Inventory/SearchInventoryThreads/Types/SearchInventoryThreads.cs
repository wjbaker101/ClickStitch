namespace ClickStitch.Api.Inventory.SearchInventoryThreads.Types;

public sealed class SearchInventoryThreadsParameters
{
    public required string? SearchTerm { get; init; }
    public required string? Brand { get; init; }
}

public sealed class SearchInventoryThreadsResponse
{
    public required List<InventoryThread> InventoryThreads { get; init; }
    public required List<ThreadModel> AvailableThreads { get; init; }

    public sealed class InventoryThread
    {
        public required ThreadModel Thread { get; init; }
        public required int Count { get; init; }
    }
}