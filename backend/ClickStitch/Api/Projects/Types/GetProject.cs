namespace ClickStitch.Api.Projects.Types;

public sealed class GetProjectResponse
{
    public required ProjectModel Project { get; init; }
    public required int AidaCount { get; init; }
    public required List<ThreadDetails> Threads { get; init; }

    public sealed class ThreadDetails
    {
        public required ThreadModel Thread { get; init; }
        public required List<StitchDetails> Stitches { get; init; }
    }

    public sealed class StitchDetails : List<int>
    {
        public StitchDetails(int x, int y)
        {
            Add(x);
            Add(y);
        }
    }
}