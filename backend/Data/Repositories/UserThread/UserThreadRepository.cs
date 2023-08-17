namespace Data.Repositories.UserThread;

public interface IUserThreadRepository : IRepository<UserThreadRecord>
{
}

public sealed class UserThreadRepository : Repository<UserThreadRecord>, IUserThreadRepository
{
    public UserThreadRepository(IDatabase database) : base(database)
    {
    }
}