using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace ClickStitch.Api.Basket;

public interface IBasketService
{
    Task<Result> QuickAdd(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
}

public sealed class BasketService : IBasketService
{
    private readonly IUserRepository _userRepository;
    private readonly IPatternRepository _patternRepository;
    private readonly IUserPatternRepository _userPatternRepository;

    public BasketService(
        IUserRepository userRepository,
        IPatternRepository patternRepository,
        IUserPatternRepository userPatternRepository)
    {
        _userRepository = userRepository;
        _patternRepository = patternRepository;
        _userPatternRepository = userPatternRepository;
    }

    public async Task<Result> QuickAdd(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result.FromFailure(patternResult);

        await _userPatternRepository.SaveAsync(new UserPatternRecord
        {
            User = user,
            Pattern = pattern,
            CreatedAt = DateTime.UtcNow,
            PausePositionX = null,
            PausePositionY = null
        }, cancellationToken);

        return Result.Success();
    }
}