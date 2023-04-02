using ClickStitch.Api.Projects.Types;
using ClickStitch.Models;
using ClickStitch.Models.Mappers;
using Core.Types;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace ClickStitch.Api.Projects;

public interface IProjectsService
{
    Task<Result<GetProjectsResponse>> GetProjects(UserModel requestUser);
    Task<Result<GetProjectResponse>> GetProject(UserModel requestUser, Guid patternReference);
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

    public async Task<Result<GetProjectsResponse>> GetProjects(UserModel requestUser)
    {
        var userResult = await _userRepository.GetByReferenceAsync(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetProjectsResponse>.FromFailure(userResult);

        var projects = await _userPatternRepository.GetByUserAsync(user);

        return new GetProjectsResponse
        {
            Projects = projects.ConvertAll(ProjectMapper.Map)
        };
    }

    public async Task<Result<GetProjectResponse>> GetProject(UserModel requestUser, Guid patternReference)
    {
        var userResult = await _userRepository.GetByReferenceAsync(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetProjectResponse>.FromFailure(userResult);

        var patternResult = await _patternRepository.GetFullByReferenceAsync(patternReference);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<GetProjectResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern);
        if (!projectResult.TrySuccess(out var project))
            return Result<GetProjectResponse>.FromFailure(projectResult);

        return new GetProjectResponse
        {
            Project = ProjectMapper.Map(project),
            AidaCount = pattern.AidaCount,
            Stitches = pattern.Stitches.Select(PatternMapper.MapStitch).ToList(),
            Threads = pattern.Threads.Select(PatternMapper.MapThread).ToList()
        };
    }
}