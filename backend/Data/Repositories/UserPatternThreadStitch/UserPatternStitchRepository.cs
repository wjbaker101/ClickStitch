namespace Data.Repositories.UserPatternThreadStitch;

public interface IUserPatternThreadStitchRepository : IRepository<UserPatternThreadStitchRecord>
{
}

public sealed class UserPatternThreadStitchRepository : Repository<UserPatternThreadStitchRecord>, IUserPatternThreadStitchRepository
{
    public UserPatternThreadStitchRepository(IDatabase database) : base(database)
    {
    }
}