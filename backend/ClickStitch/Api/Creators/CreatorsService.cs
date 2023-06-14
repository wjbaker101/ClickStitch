using ClickStitch.Api.Creators.Types;
using Core.Extensions;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.Permission;
using Data.Repositories.User;
using Data.Repositories.UserCreator;
using Data.Repositories.UserPermission;

namespace ClickStitch.Api.Creators;

public interface ICreatorsService
{
    Task<Result<CreateCreatorResponse>> CreateCreator(RequestUser requestUser, CreateCreatorRequest request, CancellationToken cancellationToken);
    Task<Result<GetCreatorResponse>> GetCreator(RequestUser requestUser, Guid creatorReference, CancellationToken cancellationToken);
}

public sealed class CreatorsService : ICreatorsService
{
    private readonly ICreatorRepository _creatorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserPermissionRepository _userPermissionRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserCreatorRepository _userCreatorRepository;

    public CreatorsService(
        ICreatorRepository creatorRepository,
        IUserRepository userRepository,
        IUserPermissionRepository userPermissionRepository,
        IPermissionRepository permissionRepository,
        IUserCreatorRepository userCreatorRepository)
    {
        _creatorRepository = creatorRepository;
        _userRepository = userRepository;
        _userPermissionRepository = userPermissionRepository;
        _permissionRepository = permissionRepository;
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
}