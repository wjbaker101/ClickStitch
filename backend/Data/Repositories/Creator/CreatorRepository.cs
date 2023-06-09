﻿using Data.Repositories.Creator.Types;

namespace Data.Repositories.Creator;

public interface ICreatorRepository : IRepository<CreatorRecord>
{
    Task<Result<CreatorRecord>> GetFullByReference(Guid creatorReference, CancellationToken cancellationToken);
    Task<Result<CreatorRecord>> GetWithUsersByReference(Guid creatorReference, CancellationToken cancellationToken);
    Task<Result<CreatorRecord>> GetByUser(UserRecord user, CancellationToken cancellationToken);
    Task<Result<GetCreatorPatternsDto>> GetCreatorPatterns(Guid creatorReference, GetCreatorPatternsParameters parameters, CancellationToken cancellationToken);
}

public sealed class CreatorRepository : Repository<CreatorRecord>, ICreatorRepository
{
    public CreatorRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Result<CreatorRecord>> GetFullByReference(Guid creatorReference, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<CreatorRecord>()
            .Where(x => x.Reference == creatorReference);

        query
            .FetchMany(x => x.Users)
            .ToFuture();

        query
            .FetchMany(x => x.Patterns)
            .ToFuture();

        var creator = (await query
            .ToFuture()
            .GetEnumerableAsync(cancellationToken))
            .SingleOrDefault();

        await transaction.CommitAsync(cancellationToken);

        if (creator == null)
            return Result<CreatorRecord>.Failure($"Unable to find creator with reference: '{creatorReference}'.");

        return creator;
    }

    public async Task<Result<CreatorRecord>> GetWithUsersByReference(Guid creatorReference, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var creator = await session
            .Query<CreatorRecord>()
            .FetchMany(x => x.Users)
            .SingleOrDefaultAsync(x => x.Reference == creatorReference, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        if (creator == null)
            return Result<CreatorRecord>.Failure($"Unable to find creator with reference: '{creatorReference}'.");

        return creator;
    }

    public async Task<Result<CreatorRecord>> GetByUser(UserRecord user, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var creator = await session
            .Query<UserCreatorRecord>()
            .Fetch(x => x.Creator)
            .Where(x => x.User == user)
            .Select(x => x.Creator)
            .SingleOrDefaultAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        if (creator == null)
            return Result<CreatorRecord>.Failure("Unable to find creator for user.");

        return creator;
    }

    public async Task<Result<GetCreatorPatternsDto>> GetCreatorPatterns(Guid creatorReference, GetCreatorPatternsParameters parameters, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var creator = await session
            .Query<CreatorRecord>()
            .SingleOrDefaultAsync(x => x.Reference == creatorReference, cancellationToken);

        if (creator == null)
            return Result<GetCreatorPatternsDto>.Failure($"Unable to find creator with reference: '{creatorReference}'.");

        var query = session
            .Query<PatternRecord>()
            .Where(x => x.Creator == creator);

        var totalCount = query.ToFutureValue(x => x.Count());

        var patterns = (await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToFuture()
            .GetEnumerableAsync(cancellationToken))
            .ToList();

        await transaction.CommitAsync(cancellationToken);

        return new GetCreatorPatternsDto
        {
            Patterns = patterns,
            TotalCount = await totalCount.GetValueAsync(cancellationToken)
        };
    }
}