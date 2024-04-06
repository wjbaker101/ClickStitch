namespace ClickStitch.Api.Patterns.GetPatternInventory.Types;

public sealed class GetPatternInventoryResponse
{
    public required Dictionary<int, InventoryThread?> Threads { get; init; }

    public sealed class InventoryThread
    {
        public required ThreadModel Thread { get; init; }
        public required int Count { get; init; }
    }
}