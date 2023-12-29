using Data.Types;

namespace Data.Repositories.UserThread;

public interface IUserThreadRepository : IRepository<UserThreadRecord>
{
    Task<List<UserThreadRecord>> GetByUser(UserRecord user, CancellationToken cancellationToken);
    Task<Result<UserThreadRecord>> GetByUserAndThread(UserRecord user, ThreadRecord thread, CancellationToken cancellationToken);
}

public sealed class UserThreadRepository : Repository<UserThreadRecord>, IUserThreadRepository
{
    public UserThreadRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<UserThreadRecord>> GetByUser(UserRecord user, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var threads = await session
            .Query<UserThreadRecord>()
            .Where(x => x.User == user)
            .ToListAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return threads;
    }

    public async Task<Result<UserThreadRecord>> GetByUserAndThread(UserRecord user, ThreadRecord thread, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var userThread = await session
            .Query<UserThreadRecord>()
            .SingleOrDefaultAsync(x => x.User == user && x.Thread == thread, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        if (userThread == null)
            return Result<UserThreadRecord>.Failure("Unable to find user's thread.");
                
        return userThread;
    }
}