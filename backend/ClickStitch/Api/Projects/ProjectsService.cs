using ClickStitch.Api.Projects.Types;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternThreadStitch;
using DotNetLibs.Core.Services;

namespace ClickStitch.Api.Projects;

public interface IProjectsService
{
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