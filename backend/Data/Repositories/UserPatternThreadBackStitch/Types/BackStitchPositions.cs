namespace Data.Repositories.UserPatternThreadBackStitch.Types;

public sealed class BackStitchPositions
{
    public required Dictionary<int, List<Position>> StitchesByThread { get; init; }

    public sealed class Position
    {
        public required int StartX { get; init; }
        public required int StartY { get; init; }
        public required int EndX { get; init; }
        public required int EndY { get; init; }
    }
}