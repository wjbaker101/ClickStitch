﻿using ClickStitch.Api.Creators.Types;
using ClickStitch.Api.Users.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.Creator.Types;
using Data.Repositories.User;
using Data.Repositories.UserCreator;

namespace ClickStitch.Api.Creators;

public interface ICreatorsService
{
    Task<Result<CreateCreatorResponse>> CreateCreator(RequestUser requestUser, CreateCreatorRequest request, CancellationToken cancellationToken);
    Task<Result<UpdateCreatorResponse>> UpdateCreator(RequestUser requestUser, Guid creatorReference, UpdateCreatorRequest request, CancellationToken cancellationToken);
    Task<Result<GetCreatorByUserResponse>> GetCreatorBySelf(RequestUser requestUser, CancellationToken cancellationToken);
    Task<Result<GetCreatorPatternsResponse>> GetCreatorPatterns(RequestUser user, Guid creatorReference, int pageSize, int pageNumber, CancellationToken cancellationToken);
}

public sealed class CreatorsService : ICreatorsService
{
    private readonly ICreatorRepository _creatorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserCreatorRepository _userCreatorRepository;

    public CreatorsService(
        ICreatorRepository creatorRepository,
        IUserRepository userRepository,
        IUserCreatorRepository userCreatorRepository)
    {
        _creatorRepository = creatorRepository;
        _userRepository = userRepository;
        _userCreatorRepository = userCreatorRepository;
    }

    public async Task<Result<CreateCreatorResponse>> CreateCreator(RequestUser requestUser, CreateCreatorRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var creator = await _creatorRepository.SaveAsync(new CreatorRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Name = request.Name,
            StoreUrl = request.StoreUrl,
            Users = new List<UserRecord>(),
            Patterns = new List<PatternRecord>()
        }, cancellationToken);

        await _userCreatorRepository.SaveAsync(new UserCreatorRecord
        {
            User = user,
            Creator = creator
        }, cancellationToken);

        return new CreateCreatorResponse
        {
            Creator = CreatorMapper.Map(creator)
        };
    }

    public async Task<Result<UpdateCreatorResponse>> UpdateCreator(RequestUser requestUser, Guid creatorReference, UpdateCreatorRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var creatorResult = await _creatorRepository.GetWithUsersByReference(creatorReference, cancellationToken);
        if (!creatorResult.TrySuccess(out var creator))
            return Result<UpdateCreatorResponse>.FromFailure(creatorResult);

        if (creator.Users.All(x => x.Id != user.Id))
            return Result<UpdateCreatorResponse>.Failure("Unable to update a creator you are not assigned to.");

        creator.Name = request.Name;
        creator.StoreUrl = request.StoreUrl;

        await _creatorRepository.UpdateAsync(creator, cancellationToken);

        return new UpdateCreatorResponse
        {
            Creator = CreatorMapper.Map(creator)
        };
    }

    public async Task<Result<GetCreatorByUserResponse>> GetCreatorBySelf(RequestUser requestUser, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var creatorResult = await _creatorRepository.GetByUser(user, cancellationToken);
        if (creatorResult.IsFailure)
        {
            return new GetCreatorByUserResponse
            {
                Creator = null
            };
        }

        return new GetCreatorByUserResponse
        {
            Creator = CreatorMapper.Map(creatorResult.Content)
        };
    }

    public async Task<Result<GetCreatorPatternsResponse>> GetCreatorPatterns(RequestUser user, Guid creatorReference, int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        var getPatternsResult = await _creatorRepository.GetCreatorPatterns(creatorReference, new GetCreatorPatternsParameters
        {
            PageSize = pageSize,
            PageNumber = pageNumber
        }, cancellationToken);

        if (!getPatternsResult.TrySuccess(out var getPatterns))
            return Result<GetCreatorPatternsResponse>.FromFailure(getPatternsResult);

        return new GetCreatorPatternsResponse
        {
            Patterns = getPatterns.Patterns.ConvertAll(PatternMapper.Map),
            Pagination = PaginationModel.Create(pageNumber, pageSize, getPatterns.TotalCount)
        };
    }
}