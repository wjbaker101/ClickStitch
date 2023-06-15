using ClickStitch.Api.Creators.Types;
using ClickStitch.Api.Users.Types;
using Core.Extensions;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.User;
using Data.Repositories.UserCreator;

namespace ClickStitch.Api.Creators;

public interface ICreatorsService
{
    Task<Result<CreateCreatorResponse>> CreateCreator(RequestUser requestUser, CreateCreatorRequest request, CancellationToken cancellationToken);
    Task<Result<GetCreatorResponse>> GetCreator(RequestUser requestUser, Guid creatorReference, CancellationToken cancellationToken);
    Task<Result<GetCreatorByUserResponse>> GetCreatorByUser(RequestUser requestUser, CancellationToken cancellationToken);
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

    public async Task<Result<GetCreatorResponse>> GetCreator(RequestUser requestUser, Guid creatorReference, CancellationToken cancellationToken)
    {
        var creatorResult = await _creatorRepository.GetFullByReference(creatorReference, cancellationToken);
        if (creatorResult.IsFailure)
            return Result<GetCreatorResponse>.FromFailure(creatorResult);

        return new GetCreatorResponse
        {
            Creator = CreatorMapper.Map(creatorResult.Content),
            Users = creatorResult.Content.Users.MapAll(UserMapper.Map),
            Patterns = creatorResult.Content.Patterns.MapAll(PatternMapper.Map)
        };
    }

    public async Task<Result<GetCreatorByUserResponse>> GetCreatorByUser(RequestUser requestUser, CancellationToken cancellationToken)
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
}