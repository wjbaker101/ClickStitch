using ClickStitch.Api.Creators.GetCreatorBySelf.Types;
using Data.Repositories.Creator;
using Data.Repositories.User;

namespace ClickStitch.Api.Creators.GetCreatorBySelf;

public interface IGetCreatorBySelfService
{
    Task<Result<GetCreatorBySelfResponse>> GetCreatorBySelf(RequestUser requestUser, CancellationToken cancellationToken);
}

public sealed class GetCreatorBySelfService : IGetCreatorBySelfService
{
    private readonly ICreatorRepository _creatorRepository;
    private readonly IUserRepository _userRepository;

    public GetCreatorBySelfService(ICreatorRepository creatorRepository, IUserRepository userRepository)
    {
        _creatorRepository = creatorRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<GetCreatorBySelfResponse>> GetCreatorBySelf(RequestUser requestUser, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var creatorResult = await _creatorRepository.GetByUser(user, cancellationToken);
        if (creatorResult.IsFailure)
        {
            return new GetCreatorBySelfResponse
            {
                Creator = null
            };
        }

        return new GetCreatorBySelfResponse
        {
            Creator = CreatorMapper.Map(creatorResult.Content)
        };
    }
}