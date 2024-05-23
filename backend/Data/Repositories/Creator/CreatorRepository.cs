using Data.Repositories.Creator.Types;
using Data.Types;

namespace Data.Repositories.Creator;

public interface ICreatorRepository : IRepository<CreatorRecord>
{
    Task<Result<CreatorRecord>> GetByReference(Guid creatorReference, CancellationToken cancellationToken);
    Task<Result<CreatorRecord>> GetWithUsersByReference(Guid creatorReference, CancellationToken cancellationToken);
    Task<Result<CreatorRecord>> GetByUser(UserRecord user, CancellationToken cancellationToken);
    Task<Result<GetCreatorPatternsDto>> GetCreatorPatterns(Guid creatorReference, GetCreatorPatternsParameters parameters, CancellationToken cancellationToken);
}

public sealed class CreatorRepository : Repository<CreatorRecord>, ICreatorRepository
{
    public CreatorRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Result<CreatorRecord>> GetByReference(Guid creatorReference, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var creator = await session
            .Query<CreatorRecord>()
            .SingleOrDefault(x => x.Reference == creatorReference, cancellationToken);

        await transaction.Commit(cancellationToken);

        if (creator == null)
            return Result<CreatorRecord>.Failure($"Unable to find creator with reference: '{creatorReference}'.");

        return creator;
    }

    public async Task<Result<CreatorRecord>> GetWithUsersByReference(Guid creatorReference, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction =  await session.BeginTransaction(cancellationToken);

        var creator = await session
            .Query<CreatorRecord>()
            .FetchMany(x => x.Users)
            .SingleOrDefault(x => x.Reference == creatorReference, cancellationToken);

        await transaction.Commit(cancellationToken);

        if (creator == null)
            return Result<CreatorRecord>.Failure($"Unable to find creator with reference: '{creatorReference}'.");

        return creator;
    }

    public async Task<Result<CreatorRecord>> GetByUser(UserRecord user, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var creator = await session
            .Query<UserCreatorRecord>()
            .Fetch(x => x.Creator)
            .Where(x => x.User == user)
            .Select(x => x.Creator)
            .SingleOrDefault(cancellationToken);

        await transaction.Commit(cancellationToken);

        if (creator == null)
            return Result<CreatorRecord>.Failure("Unable to find creator for user.");

        return creator;
    }

    public async Task<Result<GetCreatorPatternsDto>> GetCreatorPatterns(Guid creatorReference, GetCreatorPatternsParameters parameters, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var creator = await session
            .Query<CreatorRecord>()
            .SingleOrDefault(x => x.Reference == creatorReference, cancellationToken);

        if (creator == null)
            return Result<GetCreatorPatternsDto>.Failure($"Unable to find creator with reference: '{creatorReference}'.");

        var query = session
            .Query<PatternRecord>()
            .Fetch(x => x.User)
            .Where(x => x.Creator == creator);

        var totalCount = query.ToFutureValue(x => x.Count());

        var patterns = (await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToFuture()
            .GetEnumerableAsync(cancellationToken))
            .ToList();

        await transaction.Commit(cancellationToken);

        return new GetCreatorPatternsDto
        {
            Patterns = patterns,
            TotalCount = await totalCount.GetValueAsync(cancellationToken)
        };
    }
}