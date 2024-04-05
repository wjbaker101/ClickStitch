using ClickStitch.Api.Users.GetSelf.Types;
using Data.Repositories.User;
using DotNetLibs.Core.Extensions;

namespace ClickStitch.Api.Users.GetSelf;

public interface IGetSelfService
{
    Task<Result<GetSelfResponse>> GetSelf(RequestUser requestUser, CancellationToken cancellationToken);
}

public sealed class GetSelfService : IGetSelfService
{
    private readonly IUserRepository _userRepository;

    public GetSelfService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetSelfResponse>> GetSelf(RequestUser requestUser, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetWithPermissionsByReferenceAsync(requestUser.Reference, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<GetSelfResponse>.FromFailure(userResult);

        return new GetSelfResponse
        {
            User = UserMapper.Map(user),
            Permissions = user.Permissions.MapAll(PermissionMapper.Map)
        };
    }
}