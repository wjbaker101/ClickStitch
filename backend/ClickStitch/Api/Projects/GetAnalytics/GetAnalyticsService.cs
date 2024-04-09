using ClickStitch.Api.Projects.GetAnalytics.Types;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternThreadStitch;

namespace ClickStitch.Api.Projects.GetAnalytics;

public interface IGetAnalyticsService
{
    Task<Result<GetAnalyticsResponse>> GetAnalytics(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
}

public sealed class GetAnalyticsService : IGetAnalyticsService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly IPatternRepository _patternRepository;
    private readonly IUserPatternThreadStitchRepository _userPatternThreadStitchRepository;

    public GetAnalyticsService(
        IUserRepository userRepository,
        IUserPatternRepository userPatternRepository,
        IPatternRepository patternRepository,
        IUserPatternThreadStitchRepository userPatternThreadStitchRepository)
    {
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _patternRepository = patternRepository;
        _userPatternThreadStitchRepository = userPatternThreadStitchRepository;
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

        var completedStitchesPerThread = await _userPatternThreadStitchRepository.GetByUserForThreads(user, pattern.Threads, cancellationToken);
        var completedStitches = completedStitchesPerThread.Values.SelectMany(x => x).ToList();

        var grouped = completedStitches
            .GroupBy(x => x.CompletedAt.Date)
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