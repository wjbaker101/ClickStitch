namespace Data.Repositories.UserPatternStitch;

public interface IUserPatternStitchRepository : IRepository<UserPatternStitchRecord>
{
    Task<Dictionary<long, UserPatternStitchRecord>> GetByUserPattern(UserPatternRecord userPattern, CancellationToken cancellationToken);
}

public sealed class UserPatternStitchRepository : Repository<UserPatternStitchRecord>, IUserPatternStitchRepository
{
    public UserPatternStitchRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Dictionary<long, UserPatternStitchRecord>> GetByUserPattern(UserPatternRecord userPattern, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var userPatterns = session
            .Query<UserPatternStitchRecord>()
            .Where(x => x.UserPattern == userPattern)
            .ToDictionary(x => x.Stitch.Id, x => x);

        await transaction.CommitAsync(cancellationToken);

        return userPatterns;
    }
}