using Core.Types;
using Data.Records;
using NHibernate.Linq;

namespace Data.Repositories.UserPattern;

public interface IUserPatternRepository : IRepository<UserPatternRecord>
{
    Task<List<UserPatternRecord>> GetByUserAsync(UserRecord user, CancellationToken cancellationToken);
    Task<Result<UserPatternRecord>> GetByUserAndPatternAsync(UserRecord user, PatternRecord pattern, CancellationToken cancellationToken);
}

public sealed class UserPatternRepository : Repository<UserPatternRecord>, IUserPatternRepository
{
    public UserPatternRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<UserPatternRecord>> GetByUserAsync(UserRecord user, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var userPatterns = await session
            .Query<UserPatternRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.User == user)
            .ToListAsync();

        await transaction.CommitAsync();

        return userPatterns;
    }

    public async Task<Result<UserPatternRecord>> GetByUserAndPatternAsync(UserRecord user, PatternRecord pattern, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var userPattern = await session
            .Query<UserPatternRecord>()
            .Fetch(x => x.Pattern)
            .SingleOrDefaultAsync(x => x.User == user && x.Pattern == pattern);

        if (userPattern == null)
            return Result<UserPatternRecord>.Failure("Unable to find pattern for user.");

        await transaction.CommitAsync();

        return userPattern;
    }
}