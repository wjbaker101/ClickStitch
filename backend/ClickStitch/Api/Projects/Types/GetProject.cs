namespace ClickStitch.Api.Projects.Types;

public sealed class GetProjectResponse
{
    public required ProjectModel Project { get; init; }
    public required List<ThreadDetails> Threads { get; init; }

    public sealed class ThreadDetails
    {
        public required PatternThreadModel Thread { get; init; }
        public required List<StitchDetails> Stitches { get; init; }
        public required List<CompletedStitchDetails> CompletedStitches { get; init; }
    }

    public sealed class StitchDetails : List<int>
    {
        public StitchDetails(int x, int y)
        {
            Add(x);
            Add(y);
        }
    }

    public sealed class CompletedStitchDetails : List<object>
    {
        public CompletedStitchDetails(int x, int y, DateTime completedAt)
        {
            Add(x);
            Add(y);
            Add(completedAt);
        }
    }
}