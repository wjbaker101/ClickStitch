using ClickStitch.Api.Admin.Types;
using Core.Extensions;
using Data.Repositories.Admin;
using Data.Repositories.Admin.Types;

namespace ClickStitch.Api.Admin;

public interface IAdminService
{
    Task<Result<GetPermissionsResponse>> GetPermissions(CancellationToken cancellationToken);
    Task<Result<SearchUsersResponse>> SearchUsers(int pageNumber, int pageSize, CancellationToken cancellationToken);
}

public sealed class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;

    public AdminService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
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
}