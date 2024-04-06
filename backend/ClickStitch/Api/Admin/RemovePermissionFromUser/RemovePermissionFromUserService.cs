using ClickStitch.Api.Admin.RemovePermissionFromUser.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserPermission;

namespace ClickStitch.Api.Admin.RemovePermissionFromUser;

public interface IRemovePermissionFromUserService
{
    Task<Result<RemovePermissionFromUserResponse>> RemovePermissionFromUser(Guid userReference, ApiPermissionType permissionType, CancellationToken cancellationToken);
}

public sealed class RemovePermissionFromUserService : IRemovePermissionFromUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPermissionRepository _userPermissionRepository;

    public RemovePermissionFromUserService(IUserRepository userRepository, IUserPermissionRepository userPermissionRepository)
    {
        _userRepository = userRepository;
        _userPermissionRepository = userPermissionRepository;
    }

    public async Task<Result<RemovePermissionFromUserResponse>> RemovePermissionFromUser(Guid userReference, ApiPermissionType permissionType, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetWithPermissionsByReferenceAsync(userReference, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<RemovePermissionFromUserResponse>.FromFailure(userResult);

        var permission = user.Permissions.SingleOrDefault(x => x.Type == (PermissionType)permissionType);
        if (permission is null)
            return Result<RemovePermissionFromUserResponse>.Failure("Unable to remove permission as the user does not have it.");

        var userPermissionResult = await _userPermissionRepository.GetByUserAndPermission(user, permission, cancellationToken);
        if (userPermissionResult.IsFailure)
            return Result<RemovePermissionFromUserResponse>.FromFailure(userPermissionResult);

        await _userPermissionRepository.DeleteAsync(userPermissionResult.Content, cancellationToken);

        return new RemovePermissionFromUserResponse();
    }
}