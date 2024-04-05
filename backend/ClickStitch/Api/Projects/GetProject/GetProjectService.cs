using ClickStitch.Api.Projects.GetProject.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternThreadStitch;
using DotNetLibs.Core.Extensions;

namespace ClickStitch.Api.Projects.GetProject;

public interface IGetProjectService
{
    Task<Result<GetProjectResponse>> GetProject(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
}

public sealed class GetProjectService : IGetProjectService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly IPatternRepository _patternRepository;
    private readonly IUserPatternThreadStitchRepository _userPatternThreadStitchRepository;

    public GetProjectService(
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
}