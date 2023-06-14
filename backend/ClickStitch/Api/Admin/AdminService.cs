using ClickStitch.Api.Admin.Types;
using Core.Extensions;
using Data.Records;
using Data.Repositories.Admin;
using Data.Repositories.Admin.Types;
using Data.Repositories.Permission;
using Data.Repositories.User;
using Data.Repositories.UserPermission;

namespace ClickStitch.Api.Admin;

public interface IAdminService
{
    Task<Result<GetPermissionsResponse>> GetPermissions(CancellationToken cancellationToken);
    Task<Result<SearchUsersResponse>> SearchUsers(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Result<AssignPermissionToUserResponse>> AssignPermissionToUser(Guid userReference, AssignPermissionToUserRequest request, CancellationToken cancellationToken);
    Task<Result<RemovePermissionFromUserResponse>> RemovePermissionFromUser(Guid userReference, RemovePermissionFromUserRequest request, CancellationToken cancellationToken);
}

public sealed class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserPermissionRepository _userPermissionRepository;

    public AdminService(
        IAdminRepository adminRepository,
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        IUserPermissionRepository userPermissionRepository)
    {
        _adminRepository = adminRepository;
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _userPermissionRepository = userPermissionRepository;
    }

    public async Task<Result<GetPermissionsResponse>> GetPermissions(CancellationToken cancellationToken)
    {
        var permissions = await _adminRepository.GetPermissions(cancellationToken);

        return new GetPermissionsResponse
        {
            Permissions = permissions.ConvertAll(PermissionMapper.Map)
        };
    }

    public async Task<Result<SearchUsersResponse>> SearchUsers(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var getUsers = await _adminRepository.SearchUsers(new SearchUsersParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        }, cancellationToken);

        return new SearchUsersResponse
        {
            Users = getUsers.Users.ConvertAll(x => new SearchUsersResponse.UserDetails
            {
                User = UserMapper.Map(x),
                Permissions = x.Permissions.MapAll(PermissionMapper.Map)
            }),
            Pagination = PaginationModel.Create(pageNumber, pageSize, getUsers.TotalCount)
        };
    }

    public async Task<Result<AssignPermissionToUserResponse>> AssignPermissionToUser(Guid userReference, AssignPermissionToUserRequest request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetByReferenceAsync(userReference, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<AssignPermissionToUserResponse>.FromFailure(userResult);

        var permissionResult = await _permissionRepository.GetByType((PermissionType) request.PermissionType, cancellationToken);
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

    public async Task<Result<RemovePermissionFromUserResponse>> RemovePermissionFromUser(Guid userReference, RemovePermissionFromUserRequest request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetWithPermissionsByReferenceAsync(userReference, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<RemovePermissionFromUserResponse>.FromFailure(userResult);

        var permission = user.Permissions.SingleOrDefault(x => x.Type == (PermissionType)request.PermissionType);
        if (permission is null)
            return Result<RemovePermissionFromUserResponse>.Failure("Unable to remove permission as the user does not have it.");

        var userPermissionResult = await _userPermissionRepository.GetByUserAndPermission(user, permission, cancellationToken);
        if (userPermissionResult.IsFailure)
            return Result<RemovePermissionFromUserResponse>.FromFailure(userPermissionResult);

        await _userPermissionRepository.DeleteAsync(userPermissionResult.Content, cancellationToken);

        return new RemovePermissionFromUserResponse();
    }
}