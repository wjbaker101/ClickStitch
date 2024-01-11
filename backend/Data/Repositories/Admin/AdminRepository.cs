using Data.Repositories.Admin.Types;
using Data.Types;

namespace Data.Repositories.Admin;

public interface IAdminRepository
{
    Task<List<PermissionRecord>> GetPermissions(CancellationToken cancellationToken);
    Task<SearchUsersDto> SearchUsers(SearchUsersParameters parameters, CancellationToken cancellationToken);
}

public sealed class AdminRepository : Repository<IDatabaseRecord>, IAdminRepository
{
    public AdminRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<PermissionRecord>> GetPermissions(CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var permissions = await session
            .Query<PermissionRecord>()
            .ToList(cancellationToken);

        await transaction.Commit(cancellationToken);

        return permissions;
    }

    public async Task<SearchUsersDto> SearchUsers(SearchUsersParameters parameters, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var query = session
            .Query<UserRecord>();

        var totalCount = query.ToFutureValue(x => x.Count());

        var users = (await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToFuture()
            .GetEnumerableAsync(cancellationToken))
            .ToList();

        var permissionsLookup = (await session
            .Query<UserPermissionRecord>()
            .Fetch(x => x.Permission)
            .Where(x => users.Contains(x.User))
            .ToList(cancellationToken))
            .GroupBy(x => x.User.Id)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Permission).ToList());

        await transaction.Commit(cancellationToken);

        return new SearchUsersDto
        {
            Users = users,
            PermissionsLookup = permissionsLookup,
            TotalCount = await totalCount.GetValueAsync(cancellationToken)
        };
    }
}