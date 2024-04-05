using ClickStitch.Api.Projects.UnPauseStitching.Types;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace ClickStitch.Api.Projects.UnPauseStitching;

public interface IUnPauseStitchingService
{
    Task<Result<UnPauseStitchingResponse>> UnPauseStitching(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
}

public sealed class UnPauseStitchingService : IUnPauseStitchingService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly IPatternRepository _patternRepository;

    public UnPauseStitchingService(IUserRepository userRepository, IUserPatternRepository userPatternRepository, IPatternRepository patternRepository)
    {
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _patternRepository = patternRepository;
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
}