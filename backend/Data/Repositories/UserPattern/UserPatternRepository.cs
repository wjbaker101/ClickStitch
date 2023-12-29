using Data.Types;

namespace Data.Repositories.UserPattern;

public interface IUserPatternRepository : IRepository<UserPatternRecord>
{
    Task<List<UserPatternRecord>> GetByUserAsync(UserRecord user, CancellationToken cancellationToken);
    Task<Result<UserPatternRecord>> GetByUserAndPatternAsync(UserRecord user, PatternRecord pattern, CancellationToken cancellationToken);
    Task<bool> DoesProjectExistForPatternAsync(PatternRecord pattern, CancellationToken cancellationToken);
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
            .ThenFetch(x => x.User)
            .ThenFetch(x => x.UserCreator)
            .ThenFetch(x => x.Creator)
            .Where(x => x.User == user)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return userPatterns;
    }

    public async Task<Result<UserPatternRecord>> GetByUserAndPatternAsync(UserRecord user, PatternRecord pattern, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var userPattern = await session
            .Query<UserPatternRecord>()
            .Fetch(x => x.Pattern)
            .ThenFetch(x => x.User)
            .ThenFetch(x => x.UserCreator)
            .ThenFetch(x => x.Creator)
            .SingleOrDefaultAsync(x => x.User == user && x.Pattern == pattern, cancellationToken);

        if (userPattern == null)
            return Result<UserPatternRecord>.Failure("Unable to find pattern for user.");

        await transaction.CommitAsync(cancellationToken);

        return userPattern;
    }

    public async Task<bool> DoesProjectExistForPatternAsync(PatternRecord pattern, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var anyProject = await session
            .Query<UserPatternRecord>()
            .AnyAsync(x => x.Pattern == pattern, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return anyProject;
    }
}