using ClickStitch.Api.Admin.RemovePermissionFromUser;
using ClickStitch.Api.Admin.RemovePermissionFromUser.Types;
using ClickStitch.Models;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeRemovePermissionFromUserService : IRemovePermissionFromUserService
{
    public Task<Result<RemovePermissionFromUserResponse>> RemovePermissionFromUser(Guid userReference, ApiPermissionType permissionType, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}