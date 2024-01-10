using ClickStitch.Api.Admin.Types;
using Data.Records;
using Data.Repositories.Admin;
using Data.Repositories.Admin.Types;
using Data.Repositories.Permission;
using Data.Repositories.Thread;
using Data.Repositories.User;
using Data.Repositories.UserPermission;

namespace ClickStitch.Api.Admin;

public interface IAdminService
{
    Task<Result<GetPermissionsResponse>> GetPermissions(CancellationToken cancellationToken);
    Task<Result<SearchUsersResponse>> SearchUsers(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Result<AssignPermissionToUserResponse>> AssignPermissionToUser(Guid userReference, AssignPermissionToUserRequest request, CancellationToken cancellationToken);
    Task<Result<RemovePermissionFromUserResponse>> RemovePermissionFromUser(Guid userReference, ApiPermissionType permissionType, CancellationToken cancellationToken);
    Task<Result<CreateThreadResponse>> CreateThread(CreateThreadRequest request, CancellationToken cancellationToken);
    Task<Result<UpdateThreadResponse>> UpdateThread(Guid threadReference, UpdateThreadRequest request, CancellationToken cancellationToken);
    Task<Result<DeleteThreadResponse>> DeleteThread(Guid threadReference, CancellationToken cancellationToken);
}

public sealed class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserPermissionRepository _userPermissionRepository;
    private readonly IThreadRepository _threadRepository;

    public AdminService(
        IAdminRepository adminRepository,
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        IUserPermissionRepository userPermissionRepository,
        IThreadRepository threadRepository)
    {
        _adminRepository = adminRepository;
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _userPermissionRepository = userPermissionRepository;
        _threadRepository = threadRepository;
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
                Permissions = MapPermissions(x.Id, getUsers.PermissionsLookup)
            }),
            Pagination = PaginationModel.Create(pageNumber, pageSize, getUsers.TotalCount)
        };
    }

    private static List<PermissionModel> MapPermissions(long userId, Dictionary<long, List<PermissionRecord>> permissionsLookup)
    {
        if (!permissionsLookup.TryGetValue(userId, out var permissions))
            return new List<PermissionModel>();

        return permissions.ConvertAll(PermissionMapper.Map);
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

    public async Task<Result<CreateThreadResponse>> CreateThread(CreateThreadRequest request, CancellationToken cancellationToken)
    {
        var thread = await _threadRepository.SaveAsync(new ThreadRecord
        {
            Reference = Guid.NewGuid(),
            Brand = request.Brand,
            Code = request.Code,
            Colour = request.Colour
        }, cancellationToken);

        return new CreateThreadResponse
        {
            Thread = ThreadMapper.Map(thread)
        };
    }

    public async Task<Result<UpdateThreadResponse>> UpdateThread(Guid threadReference, UpdateThreadRequest request, CancellationToken cancellationToken)
    {
        var threadResult = await _threadRepository.GetByReference(threadReference, cancellationToken);
        if (!threadResult.TrySuccess(out var thread))
            return Result<UpdateThreadResponse>.FromFailure(threadResult);

        thread.Brand = request.Brand;
        thread.Code = request.Code;

        await _threadRepository.UpdateAsync(thread, cancellationToken);

        return new UpdateThreadResponse
        {
            Thread = ThreadMapper.Map(thread)
        };
    }

    public async Task<Result<DeleteThreadResponse>> DeleteThread(Guid threadReference, CancellationToken cancellationToken)
    {
        var threadResult = await _threadRepository.GetByReference(threadReference, cancellationToken);
        if (threadResult.IsFailure)
            return Result<DeleteThreadResponse>.FromFailure(threadResult);

        await _threadRepository.DeleteAsync(threadResult.Content, cancellationToken);

        return new DeleteThreadResponse();
    }
}