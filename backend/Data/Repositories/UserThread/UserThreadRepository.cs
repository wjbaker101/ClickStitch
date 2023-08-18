namespace Data.Repositories.UserThread;

public interface IUserThreadRepository : IRepository<UserThreadRecord>
{
    Task<List<UserThreadRecord>> GetByUser(UserRecord user, CancellationToken cancellationToken);
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
}