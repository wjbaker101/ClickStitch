using ClickStitch.Api.Users.GetUserBySelf.Types;
using Data.Repositories.User;
using DotNetLibs.Core.Extensions;

namespace ClickStitch.Api.Users.GetUserBySelf;

public interface IGetUserBySelfService
{
    Task<Result<GetUserBySelfResponse>> GetUserBySelf(RequestUser requestUser, CancellationToken cancellationToken);
}

public sealed class GetUserByUserBySelfService : IGetUserBySelfService
{
    private readonly IUserRepository _userRepository;

    public GetUserByUserBySelfService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetUserBySelfResponse>> GetUserBySelf(RequestUser requestUser, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetWithPermissionsByReferenceAsync(requestUser.Reference, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<GetUserBySelfResponse>.FromFailure(userResult);

        return new GetUserBySelfResponse
        {
            User = UserMapper.Map(user),
            Permissions = user.Permissions.MapAll(PermissionMapper.Map)
        };
    }
}