using ClickStitch.Api.Admin.GetPermissions;
using ClickStitch.Api.Admin.GetPermissions.Types;
using ClickStitch.Models;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeGetPermissionsService : IGetPermissionsService
{
    public Task<Result<GetPermissionsResponse>> GetPermissions(CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<GetPermissionsResponse>>(new GetPermissionsResponse
        {
            Permissions = new List<PermissionModel>
            {
                new()
                {
                    Type = ApiPermissionType.Creator,
                    Name = "Creator"
                }
            }
        });
    }
}