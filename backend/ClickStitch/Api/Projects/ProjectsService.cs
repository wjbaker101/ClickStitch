﻿using ClickStitch.Api.Projects.Types;
using Core.Extensions;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternStitch;
using Data.Repositories.UserPatternThreadStitch;
using Data.Repositories.UserPatternThreadStitch.Types;

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
    private readonly IUserPatternThreadStitchRepository _userPatternThreadStitchRepository;

    public ProjectsService(
        IUserRepository userRepository,
        IUserPatternRepository userPatternRepository,
        IPatternRepository patternRepository,
        IUserPatternStitchRepository userPatternStitchRepository,
        IUserPatternThreadStitchRepository userPatternThreadStitchRepository)
    {
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _patternRepository = patternRepository;
        _userPatternStitchRepository = userPatternStitchRepository;
        _userPatternThreadStitchRepository = userPatternThreadStitchRepository;
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
            AidaCount = pattern.AidaCount,
            Threads = pattern.Threads.MapAll(thread => new GetProjectResponse.ThreadDetails
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

        var grouped = userPatternStitches
            .Select(x => x.Value)
            .GroupBy(x => x.StitchedAt.Date)
            .ToList();

        return new GetAnalyticsResponse
        {
            Title = pattern.Title,
            ThumbnailUrl = pattern.ThumbnailUrl,
            PurchasedAt = project.CreatedAt,
            TotalStitches = pattern.StitchCount,
            CompletedStitches = userPatternStitches.Count,
            RemainingStitches = pattern.StitchCount - userPatternStitches.Count,
            Data = new GetAnalyticsResponse.DataDetails
            {
                Headings = grouped.ConvertAll(x => x.Key.ToShortDateString()),
                Values = grouped.ConvertAll(x => x.Count())
            }
        };
    }
}