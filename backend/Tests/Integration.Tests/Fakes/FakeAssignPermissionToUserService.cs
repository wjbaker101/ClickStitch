using ClickStitch.Api.Admin.AssignPermissionToUser;
using ClickStitch.Api.Admin.AssignPermissionToUser.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeAssignPermissionToUserService : IAssignPermissionToUserService
{
    public Task<Result<AssignPermissionToUserResponse>> AssignPermissionToUser(Guid userReference, AssignPermissionToUserRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}