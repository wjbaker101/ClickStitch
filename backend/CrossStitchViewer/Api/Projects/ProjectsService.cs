using Core.Types;
using CrossStitchViewer.Api.Projects.Types;
using CrossStitchViewer.Mappers;
using CrossStitchViewer.Models;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace CrossStitchViewer.Api.Projects;

public interface IProjectsService
{
    Result<GetProjectsResponse> GetProjects(UserModel requestUser);
    Result<GetProjectResponse> GetProject(UserModel requestUser, Guid patternReference);
}

public sealed class ProjectsService : IProjectsService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly IPatternRepository _patternRepository;

    public ProjectsService(IUserRepository userRepository, IUserPatternRepository userPatternRepository, IPatternRepository patternRepository)
    {
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _patternRepository = patternRepository;
    }

    public Result<GetProjectsResponse> GetProjects(UserModel requestUser)
    {
        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetProjectsResponse>.FromFailure(userResult);

        var projects = _userPatternRepository.GetByUser(user);

        return new GetProjectsResponse
        {
            Projects = projects.ConvertAll(ProjectMapper.Map)
        };
    }

    public Result<GetProjectResponse> GetProject(UserModel requestUser, Guid patternReference)
    {
        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetProjectResponse>.FromFailure(userResult);

        var patternResult = _patternRepository.GetFullByReference(patternReference);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<GetProjectResponse>.FromFailure(patternResult);

        var projectResult = _userPatternRepository.GetByUserAndPattern(user, pattern);
        if (!projectResult.TrySuccess(out var project))
            return Result<GetProjectResponse>.FromFailure(projectResult);

        return new GetProjectResponse
        {
            Project = ProjectMapper.Map(project),
            Stitches = pattern.Stitches
                .Select(x => new GetProjectResponse.Stitch
                {
                    ThreadIndex = x.ThreadIndex,
                    X = x.X,
                    Y = x.Y
                })
                .ToList(),
            Threads = pattern.Threads
                .Select(x => new GetProjectResponse.Thread
                {
                    Name = x.Name,
                    Description = x.Description,
                    Index = x.Index
                })
                .ToList()
        };
    }
}