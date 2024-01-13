namespace ClickStitch.Api.Patterns.Types;

public sealed class GetPatternInventoryResponse
{
    public required List<InventoryThread> Threads { get; init; }

    public sealed class InventoryThread
    {
        public required ThreadModel Thread { get; init; }
        public required int Count { get; init; }
    }
}