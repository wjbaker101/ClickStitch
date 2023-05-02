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
    Task<Result<GetProjectsResponse>> GetProjects(RequestUser requestUser, CancellationToken cancellationToken);
    Task<Result<GetProjectResponse>> GetProject(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
    Task<Result<CompleteStitchesResponse>> CompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken);
    Task<Result<CompleteStitchesResponse>> UnCompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken);
    Task<Result<GetAnalyticsResponse>> GetAnalytics(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
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

    public async Task<Result<GetProjectsResponse>> GetProjects(RequestUser requestUser, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var projects = await _userPatternRepository.GetByUserAsync(user, cancellationToken);

        return new GetProjectsResponse
        {
            Projects = projects.ConvertAll(ProjectMapper.Map)
        };
    }

    public async Task<Result<GetProjectResponse>> GetProject(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetFullByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<GetProjectResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern, cancellationToken);
        if (!projectResult.TrySuccess(out var project))
            return Result<GetProjectResponse>.FromFailure(projectResult);

        var userPatternStitches = await _userPatternStitchRepository.GetByUserPattern(project, cancellationToken);

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

    public async Task<Result<CompleteStitchesResponse>> CompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken)
    {
        if (request.Positions.Count > MAX_STITCH_SELECTION)
            return Result<CompleteStitchesResponse>.Failure($"The number of stitches to complete exceeds maximum ({MAX_STITCH_SELECTION}), please try again with a smaller selection.");

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetFullByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<CompleteStitchesResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern, cancellationToken);
        if (!projectResult.TrySuccess(out var project))
            return Result<CompleteStitchesResponse>.FromFailure(projectResult);

        await _userPatternStitchRepository.CompleteByPositions(pattern, project, request.Positions.Select(x => (x.X, x.Y)).ToList());

        return new CompleteStitchesResponse();
    }

    public async Task<Result<CompleteStitchesResponse>> UnCompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken)
    {
        if (request.Positions.Count > MAX_STITCH_SELECTION)
            return Result<CompleteStitchesResponse>.Failure($"The number of stitches to un-complete exceeds maximum ({MAX_STITCH_SELECTION}), please try again with a smaller selection.");

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetFullByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<CompleteStitchesResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern, cancellationToken);
        if (!projectResult.TrySuccess(out var project))
            return Result<CompleteStitchesResponse>.FromFailure(projectResult);

        await _userPatternStitchRepository.DeleteByPositions(project, request.Positions.ConvertAll(x => (x.X, x.Y)));

        return new CompleteStitchesResponse();
    }

    public async Task<Result<GetAnalyticsResponse>> GetAnalytics(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<GetAnalyticsResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern, cancellationToken);
        if (!projectResult.TrySuccess(out var project))
            return Result<GetAnalyticsResponse>.FromFailure(projectResult);

        var userPatternStitches = await _userPatternStitchRepository.GetByUserPattern(project, cancellationToken);

        return new GetAnalyticsResponse
        {
            Title = pattern.Title,
            ThumbnailUrl = pattern.ThumbnailUrl,
            PurchasedAt = project.CreatedAt,
            TotalStitches = pattern.StitchCount,
            CompletedStitches = userPatternStitches.Count,
            RemainingStitches = pattern.StitchCount - userPatternStitches.Count
        };
    }
}