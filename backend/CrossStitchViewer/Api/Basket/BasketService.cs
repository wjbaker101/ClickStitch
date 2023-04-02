﻿using Core.Types;
using CrossStitchViewer.Api.Basket.Types;
using CrossStitchViewer.Mappers;
using CrossStitchViewer.Models;
using Data.Records;
using Data.Repositories.Basket;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace CrossStitchViewer.Api.Basket;

public interface IBasketService
{
    Task<Result<GetBasketResponse>> GetBasket(UserModel requestUser);
    Task<Result<AddToBasketResponse>> AddToBasket(UserModel requestUser, Guid patternReference);
    Task<Result<RemoveFromBasketResponse>> RemoveFromBasket(UserModel requestUser, Guid patternReference);
    Task<Result<CompleteBasketResponse>> CompleteBasket(UserModel requestUser);
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

    public async Task<Result<GetBasketResponse>> GetBasket(UserModel requestUser)
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

    public async Task<Result<AddToBasketResponse>> AddToBasket(UserModel requestUser, Guid patternReference)
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

        _basketRepository.Save(new UserBasketItemRecord
        {
            User = user,
            Pattern = pattern,
            CreatedAt = DateTime.UtcNow
        });

        return new AddToBasketResponse();
    }

    public async Task<Result<RemoveFromBasketResponse>> RemoveFromBasket(UserModel requestUser, Guid patternReference)
    {
        var userResult = await _userRepository.GetByReferenceAsync(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<RemoveFromBasketResponse>.FromFailure(userResult);

        var basketItems = await _basketRepository.GetByUserAsync(user);

        var basketItemToRemove = basketItems.SingleOrDefault(x => x.Pattern.Reference == patternReference);
        if (basketItemToRemove == null)
            return Result<RemoveFromBasketResponse>.Failure("Cannot remove pattern from basket, it's not in there!");

        _basketRepository.Delete(basketItemToRemove);

        return new RemoveFromBasketResponse();
    }

    public async Task<Result<CompleteBasketResponse>> CompleteBasket(UserModel requestUser)
    {
        var userResult = await _userRepository.GetByReferenceAsync(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<CompleteBasketResponse>.FromFailure(userResult);

        var basketItems = await _basketRepository.GetByUserAsync(user);

        _userPatternRepository.SaveMany(basketItems.ConvertAll(x => new UserPatternRecord
        {
            User = user,
            Pattern = x.Pattern,
            CreatedAt = DateTime.UtcNow
        }));

        _basketRepository.DeleteMany(basketItems);

        return new CompleteBasketResponse();
    }
}