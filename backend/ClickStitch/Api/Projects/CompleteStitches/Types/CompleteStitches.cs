namespace ClickStitch.Api.Projects.CompleteStitches.Types;

public sealed class CompleteStitchesRequest
{
    public required Dictionary<int, List<Position>> StitchesByThread { get; init; }

    public sealed class Position
    {
        public required int X { get; init; }
        public required int Y { get; init; }
    }
}

public sealed class CompleteStitchesResponse
{
}