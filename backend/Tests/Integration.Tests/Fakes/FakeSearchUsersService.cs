using ClickStitch.Api.Admin.SearchUsers;
using ClickStitch.Api.Admin.SearchUsers.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeSearchUsersService : ISearchUsersService
{
    public Task<Result<SearchUsersResponse>> SearchUsers(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}