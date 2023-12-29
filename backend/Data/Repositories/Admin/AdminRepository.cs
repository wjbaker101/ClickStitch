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
            .ToListAsync(cancellationToken);

        await transaction.Commit(cancellationToken);

        return permissions;
    }

    public async Task<SearchUsersDto> SearchUsers(SearchUsersParameters parameters, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<UserRecord>();

        var totalCount = query.ToFutureValue(x => x.Count());

        var usersQuery = query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize);

        usersQuery
            .FetchMany(x => x.Permissions)
            .ToFuture();

        var users = (await usersQuery
            .ToFuture()
            .GetEnumerableAsync(cancellationToken))
            .ToList();

        await transaction.CommitAsync(cancellationToken);

        return new SearchUsersDto
        {
            Users = users,
            TotalCount = await totalCount.GetValueAsync(cancellationToken)
        };
    }
}