using ClickStitch.Api.Projects.Types;
using ClickStitch.Models;
using ClickStitch.Models.Mappers;
using Core.Types;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternStitch;

namespace ClickStitch.Api.Projects;

public interface IProjectsService
{
    Task<Result<GetProjectsResponse>> GetProjects(RequestUser requestUser);
    Task<Result<GetProjectResponse>> GetProject(RequestUser requestUser, Guid patternReference);
    Task<Result<CompleteStitchesResponse>> CompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request);
    Task<Result<CompleteStitchesResponse>> UnCompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request);
}

public sealed class ProjectsService : IProjectsService
{
    private const int MAX_STITCH_SELECTION = 100;

    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly IPatternRepository _patternRepository;
    private readonly IUserPatternStitchRepository _userPatternStitchRepository;
    private readonly IPatternStitchRepository _patternStitchRepository;

    public ProjectsService(
        IUserRepository userRepository,
        IUserPatternRepository userPatternRepository,
        IPatternRepository patternRepository,
        IUserPatternStitchRepository userPatternStitchRepository,
        IPatternStitchRepository patternStitchRepository)
    {
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _patternRepository = patternRepository;
        _userPatternStitchRepository = userPatternStitchRepository;
        _patternStitchRepository = patternStitchRepository;
    }

    public async Task<Result<GetProjectsResponse>> GetProjects(RequestUser requestUser)
    {
        var user = await _userRepository.GetByRequestUser(requestUser);

        var projects = await _userPatternRepository.GetByUserAsync(user);

        return new GetProjectsResponse
        {
            Projects = projects.ConvertAll(ProjectMapper.Map)
        };
    }

    public async Task<Result<GetProjectResponse>> GetProject(RequestUser requestUser, Guid patternReference)
    {
        var user = await _userRepository.GetByRequestUser(requestUser);

        var patternResult = await _patternRepository.GetFullByReferenceAsync(patternReference);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<GetProjectResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern);
        if (!projectResult.TrySuccess(out var project))
            return Result<GetProjectResponse>.FromFailure(projectResult);

        var userPatternStitches = await _userPatternStitchRepository.GetByUserPattern(project);

        var stitches = pattern.Stitches.Select(x =>
        {
            userPatternStitches.TryGetValue(x.Id, out var userStitch);

            return new StitchModel
            {
                ThreadIndex = x.ThreadIndex,
                X = x.X,
                Y = x.Y,
                StitchedAt = userStitch?.StitchedAt
            };
        });

        return new GetProjectResponse
        {
            Project = ProjectMapper.Map(project),
            AidaCount = pattern.AidaCount,
            Stitches = stitches.ToList(),
            Threads = pattern.Threads.Select(PatternMapper.MapThread).ToList()
        };
    }

    public async Task<Result<CompleteStitchesResponse>> CompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request)
    {
        if (request.Positions.Count > MAX_STITCH_SELECTION)
            return Result<CompleteStitchesResponse>.Failure($"The number of stitches to complete exceeds maximum ({MAX_STITCH_SELECTION}), please try again with a smaller selection.");

        var user = await _userRepository.GetByRequestUser(requestUser);

        var patternResult = await _patternRepository.GetFullByReferenceAsync(patternReference);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<CompleteStitchesResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern);
        if (!projectResult.TrySuccess(out var project))
            return Result<CompleteStitchesResponse>.FromFailure(projectResult);

        await _userPatternStitchRepository.CompleteByPositions(pattern, project, request.Positions.Select(x => (x.X, x.Y)).ToList());

        return new CompleteStitchesResponse();
    }

    public async Task<Result<CompleteStitchesResponse>> UnCompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request)
    {
        if (request.Positions.Count > MAX_STITCH_SELECTION)
            return Result<CompleteStitchesResponse>.Failure($"The number of stitches to un-complete exceeds maximum ({MAX_STITCH_SELECTION}), please try again with a smaller selection.");

        var user = await _userRepository.GetByRequestUser(requestUser);

        var patternResult = await _patternRepository.GetFullByReferenceAsync(patternReference);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<CompleteStitchesResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern);
        if (!projectResult.TrySuccess(out var project))
            return Result<CompleteStitchesResponse>.FromFailure(projectResult);

        await _userPatternStitchRepository.DeleteByPositions(project, request.Positions.ConvertAll(x => (x.X, x.Y)));

        return new CompleteStitchesResponse();
    }
}