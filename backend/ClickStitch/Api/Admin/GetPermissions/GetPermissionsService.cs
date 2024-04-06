using ClickStitch.Api.Admin.GetPermissions.Types;
using Data.Repositories.Admin;

namespace ClickStitch.Api.Admin.GetPermissions;

public interface IGetPermissionsService
{
    Task<Result<GetPermissionsResponse>> GetPermissions(CancellationToken cancellationToken);
}

public sealed class GetPermissionsService : IGetPermissionsService
{
    private readonly IAdminRepository _adminRepository;

    public GetPermissionsService(IAdminRepository adminRepository)
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
}