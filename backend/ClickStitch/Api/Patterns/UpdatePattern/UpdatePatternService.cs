using ClickStitch.Api.Patterns.UpdatePattern.Types;
using Data.Repositories.Pattern;
using Data.Repositories.User;

namespace ClickStitch.Api.Patterns.UpdatePattern;

public interface IUpdatePatternService
{
    Task<Result<UpdatePatternResponse>> UpdatePattern(RequestUser requestUser, Guid patternReference, UpdatePatternRequest request, CancellationToken cancellationToken);
}

public sealed class UpdatePatternService : IUpdatePatternService
{
    private readonly IPatternRepository _patternRepository;
    private readonly IUserRepository _userRepository;

    public UpdatePatternService(IPatternRepository patternRepository, IUserRepository userRepository)
    {
        _patternRepository = patternRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<UpdatePatternResponse>> UpdatePattern(RequestUser requestUser, Guid patternReference, UpdatePatternRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<UpdatePatternResponse>.FromFailure(patternResult);

        if (pattern.User.Id != user.Id)
            return Result<UpdatePatternResponse>.Failure("Unable to update pattern as you are not a creator of it.");

        pattern.Title = request.Title;
        pattern.AidaCount = request.AidaCount;

        if (requestUser.Permissions.IsCreator() && request.ExternalShopUrl != null)
            pattern.ExternalShopUrl = request.ExternalShopUrl;

        await _patternRepository.UpdateAsync(pattern, cancellationToken);

        return new UpdatePatternResponse
        {
            Pattern = PatternMapper.Map(pattern)
        };
    }
}