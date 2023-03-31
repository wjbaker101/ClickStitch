using CrossStitchViewer.Models;

namespace CrossStitchViewer.Api.Projects.Types;

public sealed class GetProjectResponse
{
    public required ProjectModel Project { get; init; }
    public required List<Stitch> Stitches { get; init; }
    public required List<Thread> Threads { get; init; }

    public sealed class Stitch
    {
        public required int ThreadIndex { get; init; }
        public required int X { get; init; }
        public required int Y { get; init; }
    }

    public sealed class Thread
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required int Index { get; init; }
    }
}