namespace ClickStitch.Api.Projects.CompleteBackStitches.Types;

public sealed class CompleteBackStitchesRequest
{
    public required Dictionary<int, List<BackStitch> > BackStitchesByThread { get; init; }

    public sealed class BackStitch
    {
        public required int StartX { get; init; }
        public required int StartY { get; init; }
        public required int EndX { get; init; }
        public required int EndY { get; init; }
    }
}

public sealed class CompleteBackStitchesResponse
{
}