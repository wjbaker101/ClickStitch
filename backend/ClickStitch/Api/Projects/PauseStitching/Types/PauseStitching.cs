namespace ClickStitch.Api.Projects.PauseStitching.Types;

public sealed class PauseStitchingRequest
{
    public required int X { get; init; }
    public required int Y { get; init; }
}

public sealed class PauseStitchingResponse
{
}