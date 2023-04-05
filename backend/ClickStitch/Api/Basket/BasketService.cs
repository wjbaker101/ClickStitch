using ClickStitch.Api.Basket.Types;
using ClickStitch.Helper;
using ClickStitch.Models.Mappers;
using Core.Types;
using Data.Records;
using Data.Repositories.Basket;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace ClickStitch.Api.Basket;

public interface IBasketService
{
    Task<Result<GetBasketResponse>> GetBasket(RequestUser requestUser);
    Task<Result<AddToBasketResponse>> AddToBasket(RequestUser requestUser, Guid patternReference);
    Task<Result<RemoveFromBasketResponse>> RemoveFromBasket(RequestUser requestUser, Guid patternReference);
    Task<Result<CompleteBasketResponse>> CompleteBasket(RequestUser requestUser);
}

public sealed class BasketService : IBasketService
{
    private readonly IUserRepository _userRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IPatternRepository _patternRepository;
    private readonly IUserPatternRepository _userPatternRepository;

    public BasketService(
        IUserRepository userRepository,
        IBasketRepository basketRepository,
        IPatternRepository patternRepository,
        IUserPatternRepository userPatternRepository)
    {
        _userRepository = userRepository;
        _basketRepository = basketRepository;
        _patternRepository = patternRepository;
        _userPatternRepository = userPatternRepository;
    }

    public async Task<Result<GetBasketResponse>> GetBasket(RequestUser requestUser)
    {
        var userResult = await _userRepository.GetByReferenceAsync(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetBasketResponse>.FromFailure(userResult);

        var basketItems = await _basketRepository.GetByUserAsync(user);

        var totalPrice = basketItems.Sum(x => x.Pattern.Price);

        return new GetBasketResponse
        {
            Basket = BasketMapper.Map(basketItems, totalPrice)
        };
    }

    public async Task<Result<AddToBasketResponse>> AddToBasket(RequestUser requestUser, Guid patternReference)
    {
        var userResult = await _userRepository.GetByReferenceAsync(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<AddToBasketResponse>.FromFailure(userResult);

        var basketItems = await _basketRepository.GetByUserAsync(user);

        if (basketItems.Any(x => x.Pattern.Reference == patternReference))
            return Result<AddToBasketResponse>.Failure("Cannot add pattern to basket, you already have it!");

        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<AddToBasketResponse>.FromFailure(patternResult);

        await _basketRepository.SaveAsync(new UserBasketItemRecord
        {
            User = user,
            Pattern = pattern,
            CreatedAt = DateTime.UtcNow
        });

        return new AddToBasketResponse();
    }

    public async Task<Result<RemoveFromBasketResponse>> RemoveFromBasket(RequestUser requestUser, Guid patternReference)
    {
        var userResult = await _userRepository.GetByReferenceAsync(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<RemoveFromBasketResponse>.FromFailure(userResult);

        var basketItems = await _basketRepository.GetByUserAsync(user);

        var basketItemToRemove = basketItems.SingleOrDefault(x => x.Pattern.Reference == patternReference);
        if (basketItemToRemove == null)
            return Result<RemoveFromBasketResponse>.Failure("Cannot remove pattern from basket, it's not in there!");

        await _basketRepository.DeleteAsync(basketItemToRemove);

        return new RemoveFromBasketResponse();
    }

    public async Task<Result<CompleteBasketResponse>> CompleteBasket(RequestUser requestUser)
    {
        var userResult = await _userRepository.GetByReferenceAsync(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<CompleteBasketResponse>.FromFailure(userResult);

        var basketItems = await _basketRepository.GetByUserAsync(user);

        await _userPatternRepository.SaveManyAsync(basketItems.ConvertAll(x => new UserPatternRecord
        {
            User = user,
            Pattern = x.Pattern,
            CreatedAt = DateTime.UtcNow
        }));

        await _basketRepository.DeleteManyAsync(basketItems);

        return new CompleteBasketResponse();
    }
}