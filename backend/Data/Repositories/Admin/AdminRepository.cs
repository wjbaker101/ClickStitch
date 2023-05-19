using Data.Records.Types;
using Data.Repositories.Admin.Types;

namespace Data.Repositories.Admin;

public interface IAdminRepository
{
    Task<SearchUsersDto> SearchUsers(SearchUsersParameters parameters, CancellationToken cancellationToken);
}

public sealed class AdminRepository : Repository<IDatabaseRecord>, IAdminRepository
{
    public AdminRepository(IDatabase database) : base(database)
    {
    }

    public async Task<SearchUsersDto> SearchUsers(SearchUsersParameters parameters, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<UserRecord>();

        var totalCount = query.ToFutureValue(x => x.Count());

        var users = (await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
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