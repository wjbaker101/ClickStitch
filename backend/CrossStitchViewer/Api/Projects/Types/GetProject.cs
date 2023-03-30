using CrossStitchViewer.Models;

namespace CrossStitchViewer.Api.Projects.Types;

public sealed class GetProjectResponse
{
    public required ProjectModel Project { get; init; }
}