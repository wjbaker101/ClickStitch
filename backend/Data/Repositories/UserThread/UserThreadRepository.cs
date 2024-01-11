using Data.Repositories.UserThread.Types;
using Data.Types;

namespace Data.Repositories.UserThread;

public interface IUserThreadRepository : IRepository<UserThreadRecord>
{
    Task<List<UserThreadRecord>> Search(UserRecord user, SearchUserThreadsParameters parameters, CancellationToken cancellationToken);
    Task<Result<UserThreadRecord>> GetByUserAndThread(UserRecord user, ThreadRecord thread, CancellationToken cancellationToken);
}

public sealed class UserThreadRepository : Repository<UserThreadRecord>, IUserThreadRepository
{
    public UserThreadRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<UserThreadRecord>> Search(UserRecord user, SearchUserThreadsParameters parameters, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var query = session
            .Query<UserThreadRecord>()
            .Fetch(x => x.Thread)
            .Where(x => x.User == user);

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            query = query.Where(x => x.Thread.Code.Contains(parameters.SearchTerm));

        if (!string.IsNullOrWhiteSpace(parameters.Brand))
            query = query.Where(x => x.Thread.Brand == parameters.Brand);

        var threads = await query
            .OrderByDescending(x => x.Count)
            .ToList(cancellationToken);

        await transaction.Commit(cancellationToken);

        return threads;
    }

    public async Task<Result<UserThreadRecord>> GetByUserAndThread(UserRecord user, ThreadRecord thread, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var userThread = await session
            .Query<UserThreadRecord>()
            .SingleOrDefault(x => x.User == user && x.Thread == thread, cancellationToken);

        await transaction.Commit(cancellationToken);

        if (userThread == null)
            return Result<UserThreadRecord>.Failure("Unable to find user's thread.");
                
        return userThread;
    }
}