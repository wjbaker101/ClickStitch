using ClickStitch.Api.Projects.AddProject.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using DotNetLibs.Core.Services;

namespace ClickStitch.Api.Projects.AddProject;

public interface IAddProjectService
{
    Task<Result<AddProjectResponse>> AddProject(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
}

public sealed class AddProjectService : IAddProjectService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly IPatternRepository _patternRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IGuidProvider _guidProvider;

    public AddProjectService(
        IUserRepository userRepository,
        IUserPatternRepository userPatternRepository,
        IPatternRepository patternRepository,
        IDateTimeProvider dateTimeProvider,
        IGuidProvider guidProvider)
    {
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _patternRepository = patternRepository;
        _dateTimeProvider = dateTimeProvider;
        _guidProvider = guidProvider;
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
            Reference = _guidProvider.NewGuid(),
            CreatedAt = _dateTimeProvider.UtcNow(),
            PausePositionX = null,
            PausePositionY = null
        }, cancellationToken);

        return new AddProjectResponse();
    }
}