using ClickStitch.Api.Admin.AssignPermissionToUser.Types;
using Data.Records;
using Data.Repositories.Permission;
using Data.Repositories.User;
using Data.Repositories.UserPermission;

namespace ClickStitch.Api.Admin.AssignPermissionToUser;

public interface IAssignPermissionToUserService
{
    Task<Result<AssignPermissionToUserResponse>> AssignPermissionToUser(Guid userReference, AssignPermissionToUserRequest request, CancellationToken cancellationToken);
}

public sealed class AssignPermissionToUserService : IAssignPermissionToUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserPermissionRepository _userPermissionRepository;

    public AssignPermissionToUserService(IUserRepository userRepository, IPermissionRepository permissionRepository, IUserPermissionRepository userPermissionRepository)
    {
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _userPermissionRepository = userPermissionRepository;
    }

    public async Task<Result<AssignPermissionToUserResponse>> AssignPermissionToUser(Guid userReference, AssignPermissionToUserRequest request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetByReferenceAsync(userReference, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<AssignPermissionToUserResponse>.FromFailure(userResult);

        var permissionResult = await _permissionRepository.GetByType((PermissionType)request.PermissionType, cancellationToken);
        if (!permissionResult.TrySuccess(out var permission))
            return Result<AssignPermissionToUserResponse>.FromFailure(permissionResult);

        await _userPermissionRepository.SaveAsync(new UserPermissionRecord
        {
            User = user,
            Permission = permission,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        return new AssignPermissionToUserResponse();
    }
}