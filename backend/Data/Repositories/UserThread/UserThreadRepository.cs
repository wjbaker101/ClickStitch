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
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<UserThreadRecord>()
            .Fetch(x => x.Thread)
            .Where(x => x.User == user);

        if (parameters.SearchTerm?.Length > 0)
            query = query.Where(x => x.Thread.Code.Contains(parameters.SearchTerm));
        
        var threads = await query.ToListAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

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