using ClickStitch.Api.Admin;
using ClickStitch.Api.Admin.Types;
using ClickStitch.Models;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeAdminService : IAdminService
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

    public Task<Result<SearchUsersResponse>> SearchUsers(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AssignPermissionToUserResponse>> AssignPermissionToUser(Guid userReference, AssignPermissionToUserRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<RemovePermissionFromUserResponse>> RemovePermissionFromUser(Guid userReference, ApiPermissionType permissionType, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<CreateThreadResponse>> CreateThread(CreateThreadRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<UpdateThreadResponse>> UpdateThread(Guid threadReference, UpdateThreadRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<DeleteThreadResponse>> DeleteThread(Guid threadReference, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}