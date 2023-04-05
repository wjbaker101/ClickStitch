namespace ClickStitch.Api.Projects.Types;

public sealed class CompleteStitchesRequest
{
    public required List<Position> Positions { get; init; }

    public sealed class Position
    {
        public required int X { get; init; }
        public required int Y { get; init; }
    }
}

public sealed class CompleteStitchesResponse
{
}