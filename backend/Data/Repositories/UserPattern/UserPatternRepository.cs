using Data.Types;

namespace Data.Repositories.UserPattern;

public interface IUserPatternRepository : IRepository<UserPatternRecord>
{
    Task<Result<UserPatternRecord>> GetByReference(Guid projectReference, CancellationToken cancellationToken);
    Task<List<UserPatternRecord>> GetByUserAsync(UserRecord user, CancellationToken cancellationToken);
    Task<Result<UserPatternRecord>> GetByUserAndPatternAsync(UserRecord user, PatternRecord pattern, CancellationToken cancellationToken);
    Task<bool> DoesProjectExistForPatternAsync(PatternRecord pattern, CancellationToken cancellationToken);
}

public sealed class UserPatternRepository : Repository<UserPatternRecord>, IUserPatternRepository
{
    public UserPatternRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Result<UserPatternRecord>> GetByReference(Guid projectReference, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var userPattern = await session
            .Query<UserPatternRecord>()
            .Fetch(x => x.Pattern)
            .SingleOrDefault(x => x.Reference == projectReference, cancellationToken);

        await transaction.Commit(cancellationToken);

        if (userPattern == null)
            return Result<UserPatternRecord>.Failure($"Unable to find project with reference: '{projectReference}'.");

        return userPattern;
    }

    public async Task<List<UserPatternRecord>> GetByUserAsync(UserRecord user, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var userPatterns = await session
            .Query<UserPatternRecord>()
            .Fetch(x => x.Pattern)
            .ThenFetch(x => x.User)
            .Fetch(x => x.Pattern)
            .ThenFetch(x => x.Creator)
            .Where(x => x.User == user)
            .OrderByDescending(x => x.CreatedAt)
            .ToList(cancellationToken);

        await transaction.Commit(cancellationToken);

        return userPatterns;
    }

    public async Task<Result<UserPatternRecord>> GetByUserAndPatternAsync(UserRecord user, PatternRecord pattern, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var userPattern = await session
            .Query<UserPatternRecord>()
            .Fetch(x => x.Pattern)
            .ThenFetch(x => x.User)
            .Fetch(x => x.Pattern)
            .ThenFetch(x => x.Creator)
            .SingleOrDefault(x => x.User == user && x.Pattern == pattern, cancellationToken);

        if (userPattern == null)
            return Result<UserPatternRecord>.Failure("Unable to find pattern for user.");

        await transaction.Commit(cancellationToken);

        return userPattern;
    }

    public async Task<bool> DoesProjectExistForPatternAsync(PatternRecord pattern, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var anyProject = await session
            .Query<UserPatternRecord>()
            .Any(x => x.Pattern == pattern, cancellationToken);

        await transaction.Commit(cancellationToken);

        return anyProject;
    }
}