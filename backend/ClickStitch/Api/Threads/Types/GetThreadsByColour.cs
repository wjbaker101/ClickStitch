namespace ClickStitch.Api.Threads.Types;

public sealed class GetThreadsByColourRequest
{
    public required List<Colour> Colours { get; init; }

    public sealed class Colour
    {
        public required int R { get; init; }
        public required int G { get; init; }
        public required int B { get; init; }
    }
}

public sealed class GetThreadsByColourResponse
{
    public required List<ThreadModel> Threads { get; init; }
}