using ClickStitch.Api.Projects.PauseStitching.Types;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace ClickStitch.Api.Projects.PauseStitching;

public interface IPauseStitchingService
{
    Task<Result<PauseStitchingResponse>> PauseStitching(RequestUser requestUser, Guid patternReference, PauseStitchingRequest request, CancellationToken cancellationToken);
}

public sealed class PauseStitchingService : IPauseStitchingService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly IPatternRepository _patternRepository;

    public PauseStitchingService(IUserRepository userRepository, IUserPatternRepository userPatternRepository, IPatternRepository patternRepository)
    {
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _patternRepository = patternRepository;
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
}