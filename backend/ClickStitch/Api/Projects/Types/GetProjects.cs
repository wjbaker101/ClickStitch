using CrossStitchViewer.Models;

namespace CrossStitchViewer.Api.Projects.Types;

public sealed class GetProjectsResponse
{
    public required List<ProjectModel> Projects { get; init; }
}