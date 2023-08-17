namespace Data.Repositories.Thread;

public interface IThreadRepository : IRepository<ThreadRecord>
{
}

public sealed class ThreadRepository : Repository<ThreadRecord>, IThreadRepository
{
    public ThreadRepository(IDatabase database) : base(database)
    {
    }
}