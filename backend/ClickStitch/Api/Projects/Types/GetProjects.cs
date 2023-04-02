using ClickStitch.Models;

namespace ClickStitch.Api.Projects.Types;

public sealed class GetProjectsResponse
{
    public required List<ProjectModel> Projects { get; init; }
}