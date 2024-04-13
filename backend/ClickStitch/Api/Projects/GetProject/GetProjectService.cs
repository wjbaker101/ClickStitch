using ClickStitch.Api.Projects.GetProject.Types;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternThreadBackStitch;
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
    private readonly IUserPatternThreadBackStitchRepository _userPatternThreadBackStitchRepository;

    public GetProjectService(
        IUserRepository userRepository,
        IUserPatternRepository userPatternRepository,
        IPatternRepository patternRepository,
        IUserPatternThreadStitchRepository userPatternThreadStitchRepository,
        IUserPatternThreadBackStitchRepository userPatternThreadBackStitchRepository)
    {
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _patternRepository = patternRepository;
        _userPatternThreadStitchRepository = userPatternThreadStitchRepository;
        _userPatternThreadBackStitchRepository = userPatternThreadBackStitchRepository;
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

        var userStitchesPerThread = await _userPatternThreadStitchRepository.GetByUserForThreads(user, pattern.Threads, cancellationToken);
        var userBackStitchesPerThread = await _userPatternThreadBackStitchRepository.GetByUserForThreads(user, pattern.Threads, cancellationToken);

        var threads = new List<GetProjectResponse.ThreadDetails>();
        foreach (var thread in pattern.Threads.OrderBy(x => x.Index))
        {
            var userStitches = userStitchesPerThread.TryGetValue(thread.Id, out var existsUserStitches) ? existsUserStitches : [];
            var userStitchLookup = userStitches.Select(x => (x.X, x.Y)).ToHashSet();

            var userBackStitches = userBackStitchesPerThread.TryGetValue(thread.Id, out var existsUserBackStitches) ? existsUserBackStitches : [];
            var userBackStitchLookup = userBackStitches.Select(x => (x.StartX, x.StartY, x.EndX, x.EndY)).ToHashSet();

            threads.Add(new GetProjectResponse.ThreadDetails
            {
                Thread = PatternMapper.MapThread(thread),
                Stitches = thread.Stitches
                    .Where(stitch => !userStitchLookup.Contains((stitch[0], stitch[1])))
                    .MapAll(x => new GetProjectResponse.StitchDetails(x[0], x[1])),
                CompletedStitches = userStitches.ConvertAll(x => new GetProjectResponse.CompletedStitchDetails(x.X, x.Y, x.CompletedAt)),
                BackStitches = thread.BackStitches
                    .Where(backStitch => !userBackStitchLookup.Contains((backStitch[0], backStitch[1], backStitch[2], backStitch[3])))
                    .MapAll(x => new GetProjectResponse.BackStitchDetails(x[0], x[1], x[2], x[3])),
                CompletedBackStitches = userBackStitches.ConvertAll(x => new GetProjectResponse.CompletedBackStitchDetails(x.StartX, x.StartY, x.EndX, x.EndY, x.CompletedAt))
            });
        }

        return new GetProjectResponse
        {
            Project = ProjectMapper.Map(project),
            Threads = threads
        };
    }
}