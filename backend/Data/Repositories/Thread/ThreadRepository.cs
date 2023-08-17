namespace Data.Repositories.Thread;

public interface IThreadRepository : IRepository<ThreadRecord>
{
    Task<List<ThreadRecord>> GetAll(CancellationToken cancellationToken);
}

public sealed class ThreadRepository : Repository<ThreadRecord>, IThreadRepository
{
    public ThreadRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<ThreadRecord>> GetAll(CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var threads = await session
            .Query<ThreadRecord>()
            .ToListAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return threads;
    }
}