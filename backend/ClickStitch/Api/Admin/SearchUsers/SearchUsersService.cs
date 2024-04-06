using ClickStitch.Api.Admin.SearchUsers.Types;
using Data.Records;
using Data.Repositories.Admin;
using Data.Repositories.Admin.Types;

namespace ClickStitch.Api.Admin.SearchUsers;

public interface ISearchUsersService
{
    Task<Result<SearchUsersResponse>> SearchUsers(int pageNumber, int pageSize, CancellationToken cancellationToken);
}

public sealed class SearchUsersService : ISearchUsersService
{
    private readonly IAdminRepository _adminRepository;

    public SearchUsersService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
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
}