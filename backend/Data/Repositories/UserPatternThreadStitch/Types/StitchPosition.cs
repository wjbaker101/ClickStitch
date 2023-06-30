namespace Data.Repositories.UserPatternThreadStitch.Types;

public sealed class StitchPosition
{
    public required Dictionary<int, List<Position>> StitchesByThread { get; init; }

    public sealed class Position
    {
        public required int X { get; init; }
        public required int Y { get; init; }
    }
}