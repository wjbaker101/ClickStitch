using ClickStitch.Api.Creators.UpdateCreator.Types;
using Data.Repositories.Creator;
using Data.Repositories.User;

namespace ClickStitch.Api.Creators.UpdateCreator;

public interface IUpdateCreatorService
{
    Task<Result<UpdateCreatorResponse>> UpdateCreator(RequestUser requestUser, Guid creatorReference, UpdateCreatorRequest request, CancellationToken cancellationToken);
}

public sealed class UpdateCreatorService : IUpdateCreatorService
{
    private readonly ICreatorRepository _creatorRepository;
    private readonly IUserRepository _userRepository;

    public UpdateCreatorService(ICreatorRepository creatorRepository, IUserRepository userRepository)
    {
        _creatorRepository = creatorRepository;
        _userRepository = userRepository;
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
        creator.Description = request.Description;

        await _creatorRepository.UpdateAsync(creator, cancellationToken);

        return new UpdateCreatorResponse
        {
            Creator = CreatorMapper.Map(creator)
        };
    }
}