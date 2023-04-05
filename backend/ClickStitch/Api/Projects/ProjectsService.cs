using ClickStitch.Api.Projects.Types;
using ClickStitch.Helper;
using ClickStitch.Models;
using ClickStitch.Models.Mappers;
using Core.Types;
using Data.Records;
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
}

public sealed class ProjectsService : IProjectsService
{
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
        var userResult = await _userRepository.GetByReferenceAsync(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetProjectsResponse>.FromFailure(userResult);

        var projects = await _userPatternRepository.GetByUserAsync(user);

        return new GetProjectsResponse
        {
            Projects = projects.ConvertAll(ProjectMapper.Map)
        };
    }

    public async Task<Result<GetProjectResponse>> GetProject(RequestUser requestUser, Guid patternReference)
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
        var userResult = await _userRepository.GetByReferenceAsync(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<CompleteStitchesResponse>.FromFailure(userResult);

        var patternResult = await _patternRepository.GetFullByReferenceAsync(patternReference);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<CompleteStitchesResponse>.FromFailure(patternResult);

        var projectResult = await _userPatternRepository.GetByUserAndPatternAsync(user, pattern);
        if (!projectResult.TrySuccess(out var project))
            return Result<CompleteStitchesResponse>.FromFailure(projectResult);

        var stitches = new List<UserPatternStitchRecord>();
        foreach (var position in request.Positions)
        {
            var stitchResult = await _patternStitchRepository.GetByPosition(pattern, position.X, position.Y);
            if (!stitchResult.TrySuccess(out var stitch))
                return Result<CompleteStitchesResponse>.FromFailure(stitchResult);

            stitches.Add(new UserPatternStitchRecord
            {
                UserPattern = project,
                Stitch = stitch,
                StitchedAt = DateTime.UtcNow,
                X = stitch.X,
                Y = stitch.Y
            });
        }

        await _userPatternStitchRepository.SaveManyAsync(stitches);

        return new CompleteStitchesResponse();
    }
}