namespace ClickStitch.Api.Projects.GetProjects.Types;

public sealed class GetProjectsResponse
{
    public required List<ProjectModel> Projects { get; init; }
}