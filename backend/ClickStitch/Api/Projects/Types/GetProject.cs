namespace ClickStitch.Api.Projects.Types;

public sealed class GetProjectResponse
{
    public required ProjectModel Project { get; init; }
    public required int AidaCount { get; init; }
    public required List<StitchModel> Stitches { get; init; }
    public required List<ThreadModel> Threads { get; init; }
}