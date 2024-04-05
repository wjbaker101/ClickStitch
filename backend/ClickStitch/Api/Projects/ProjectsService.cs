using ClickStitch.Api.Projects.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternThreadStitch;
using Data.Repositories.UserPatternThreadStitch.Types;
using DotNetLibs.Core.Extensions;
using DotNetLibs.Core.Services;

namespace ClickStitch.Api.Projects;

public interface IProjectsService
{
    Task<Result<AddProjectResponse>> AddProject(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
    Task<Result<GetProjectResponse>> GetProject(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
    Task<Result<CompleteStitchesResponse>> CompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken);
    Task<Result<CompleteStitchesResponse>> UnCompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken);
    Task<Result<PauseStitchingResponse>> PauseStitching(RequestUser requestUser, Guid patternReference, PauseStitchingRequest request, CancellationToken cancellationToken);
    Task<Result<UnPauseStitchingResponse>> UnPauseStitching(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
    Task<Result<GetAnalyticsResponse>> GetAnalytics(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
}

public sealed class ProjectsService : IProjectsService
{
    private const int MAX_STITCH_SELECTION = 100;

    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly IPatternRepository _patternRepository;
    private readonly IUserPatternThreadStitchRepository _userPatternThreadStitchRepository;
    private readonly IDateTimeProvider _dateTime;
    private readonly IGuidProvider _guid;

    public ProjectsService(
        IUserRepository userRepository,
        IUserPatternRepository userPatternRepository,
        IPatternRepository patternRepository,
        IUserPatternThreadStitchRepository userPatternThreadStitchRepository,
        IDateTimeProvider dateTime,
        IGuidProvider guid)
    {
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _patternRepository = patternRepository;
        _userPatternThreadStitchRepository = userPatternThreadStitchRepository;
        _dateTime = dateTime;
        _guid = guid;
    }

    public async Task<Result<AddProjectResponse>> AddProject(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<AddProjectResponse>.FromFailure(patternResult);

        await _userPatternRepository.SaveAsync(new UserPatternRecord
        {
            User = user,
            Pattern = pattern,
            Reference = _guid.NewGuid(),
            CreatedAt = _dateTime.UtcNow(),
            PausePositionX = null,
            PausePositionY = null
        }, cancellationToken);

        return new AddProjectResponse();
    }

    public async Task<Result<GetProjectResponse>> GetProject(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetWithThreadsByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<GetProjectResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern, cancellationToken);
        if (!projectResult.TrySuccess(out var project))
            return Result<GetProjectResponse>.FromFailure(projectResult);

        var stitches = await _patternRepository.GetStitchesByThreads(pattern.Threads.ToList(), cancellationToken);

        var userStitches = await _userPatternThreadStitchRepository.GetByUser(user, patternReference, cancellationToken);

        return new GetProjectResponse
        {
            Project = ProjectMapper.Map(project),
            Threads = pattern.Threads
                .OrderBy(x => x.Index)
                .MapAll(thread => new GetProjectResponse.ThreadDetails
                {
                    Thread = PatternMapper.MapThread(thread),
                    Stitches = MapStitches(stitches[thread.Index], userStitches, thread.Index),
                    CompletedStitches = MapCompletedStitches(userStitches, thread.Index)
                })
        };
    }

    private static List<GetProjectResponse.StitchDetails> MapStitches(List<PatternThreadStitchRecord> stitches, Dictionary<int, Dictionary<long, UserPatternThreadStitchRecord>> userStitches, int threadIndex)
    {
        return stitches
            .Where(x => !(userStitches.TryGetValue(threadIndex, out var userStitch) && userStitch.ContainsKey(x.Id)))
            .MapAll(stitch => new GetProjectResponse.StitchDetails(stitch.X, stitch.Y));
    }

    private static List<GetProjectResponse.CompletedStitchDetails> MapCompletedStitches(Dictionary<int, Dictionary<long, UserPatternThreadStitchRecord>> userStitches, int threadIndex)
    {
        if (!userStitches.TryGetValue(threadIndex, out var threadStitches))
            return new List<GetProjectResponse.CompletedStitchDetails>();

        return threadStitches.Values.MapAll(x => new GetProjectResponse.CompletedStitchDetails(x.Stitch.X, x.Stitch.Y, x.StitchedAt));
    }

    public async Task<Result<CompleteStitchesResponse>> CompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken)
    {
        if (request.StitchesByThread.Sum(x => x.Value.Count) > MAX_STITCH_SELECTION)
            return Result<CompleteStitchesResponse>.Failure($"The number of stitches to complete exceeds maximum ({MAX_STITCH_SELECTION}), please try again with a smaller selection.");

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        await _userPatternThreadStitchRepository.Complete(user, patternReference, new StitchPosition
        {
            StitchesByThread = request.StitchesByThread.ToDictionary(x => x.Key, x => x.Value.ConvertAll(pos => new StitchPosition.Position
            {
                X = pos.X,
                Y = pos.Y
            }))
        }, cancellationToken);

        return new CompleteStitchesResponse();
    }

    public async Task<Result<CompleteStitchesResponse>> UnCompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken)
    {
        if (request.StitchesByThread.Sum(x => x.Value.Count) > MAX_STITCH_SELECTION)
            return Result<CompleteStitchesResponse>.Failure($"The number of stitches to un-complete exceeds maximum ({MAX_STITCH_SELECTION}), please try again with a smaller selection.");

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        await _userPatternThreadStitchRepository.UnComplete(user, patternReference, new StitchPosition
        {
            StitchesByThread = request.StitchesByThread.ToDictionary(x => x.Key, x => x.Value.ConvertAll(pos => new StitchPosition.Position
            {
                X = pos.X,
                Y = pos.Y
            }))
        }, cancellationToken);

        return new CompleteStitchesResponse();
    }
    
    public async Task<Result<PauseStitchingResponse>> PauseStitching(RequestUser requestUser, Guid patternReference, PauseStitchingRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<PauseStitchingResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern, cancellationToken);
        if (!projectResult.TrySuccess(out var project))
            return Result<PauseStitchingResponse>.FromFailure(projectResult);

        project.PausePositionX = request.X;
        project.PausePositionY = request.Y;

        await _userPatternRepository.UpdateAsync(project, cancellationToken);

        return new PauseStitchingResponse();
    }
    
    public async Task<Result<UnPauseStitchingResponse>> UnPauseStitching(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<UnPauseStitchingResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern, cancellationToken);
        if (!projectResult.TrySuccess(out var project))
            return Result<UnPauseStitchingResponse>.FromFailure(projectResult);

        project.PausePositionX = null;
        project.PausePositionY = null;

        await _userPatternRepository.UpdateAsync(project, cancellationToken);

        return new UnPauseStitchingResponse();
    }

    public async Task<Result<GetAnalyticsResponse>> GetAnalytics(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetWithThreadsByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<GetAnalyticsResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern, cancellationToken);
        if (!projectResult.TrySuccess(out var project))
            return Result<GetAnalyticsResponse>.FromFailure(projectResult);

        var completedStitchesByThreads = await _userPatternThreadStitchRepository.GetByUser(user, patternReference, cancellationToken);
        var completedStitches = completedStitchesByThreads.Values.SelectMany(x => x.Values).ToList();

        var grouped = completedStitches
            .GroupBy(x => x.StitchedAt.Date)
            .ToList();

        return new GetAnalyticsResponse
        {
            Title = pattern.Title,  
            ThumbnailUrl = pattern.ThumbnailUrl,
            BannerImageUrl = pattern.BannerImageUrl,
            PurchasedAt = project.CreatedAt,
            TotalStitches = pattern.StitchCount,
            CompletedStitches = completedStitches.Count,
            RemainingStitches = pattern.StitchCount - completedStitches.Count,
            Data = new GetAnalyticsResponse.DataDetails
            {
                Headings = grouped.ConvertAll(x => x.Key.ToShortDateString()),
                Values = grouped.ConvertAll(x => x.Count())
            }
        };
    }
}