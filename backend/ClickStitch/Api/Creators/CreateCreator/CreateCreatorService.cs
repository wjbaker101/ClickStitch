using ClickStitch.Api.Creators.CreateCreator.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.User;
using Data.Repositories.UserCreator;

namespace ClickStitch.Api.Creators.CreateCreator;

public interface ICreateCreatorService
{
    Task<Result<CreateCreatorResponse>> CreateCreator(RequestUser requestUser, CreateCreatorRequest request, CancellationToken cancellationToken);
}

public sealed class CreateCreatorService : ICreateCreatorService
{
    private readonly ICreatorRepository _creatorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserCreatorRepository _userCreatorRepository;

    public CreateCreatorService(
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
            Description = null,
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
}