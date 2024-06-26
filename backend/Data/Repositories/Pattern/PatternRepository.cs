﻿using Data.Repositories.Pattern.Types;
using Data.Types;

namespace Data.Repositories.Pattern;

public interface IPatternRepository : IRepository<PatternRecord>
{
    Task<List<PatternRecord>> SearchAsync(SearchPatternsParameters parameters, CancellationToken cancellationToken);
    Task<Result<PatternRecord>> GetByReferenceAsync(Guid patternReference, CancellationToken cancellationToken);
    Task<Result<PatternRecord>> GetWithThreadsByReferenceAsync(Guid patternReference, CancellationToken cancellationToken);
}

public sealed class PatternRepository : Repository<PatternRecord>, IPatternRepository
{
    public PatternRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<PatternRecord>> SearchAsync(SearchPatternsParameters parameters, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var patterns = await session
            .Query<PatternRecord>()
            .Fetch(x => x.User)
            .Fetch(x => x.Creator)
            .Where(x => !parameters.PatternsToExclude.Contains(x) && x.IsPublic)
            .ToList(cancellationToken);

        await transaction.Commit(cancellationToken);

        return patterns;
    }

    public async Task<Result<PatternRecord>> GetByReferenceAsync(Guid patternReference, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var pattern = await session
            .Query<PatternRecord>()
            .Fetch(x => x.User)
            .Fetch(x => x.Creator)
            .SingleOrDefault(x => x.Reference == patternReference, cancellationToken);

        if (pattern == null)
            return Result<PatternRecord>.Failure($"Unable to find pattern with reference: '{patternReference}'.");

        await transaction.Commit(cancellationToken);

        return pattern;
    }

    public async Task<Result<PatternRecord>> GetWithThreadsByReferenceAsync(Guid patternReference, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var pattern = await session
            .Query<PatternRecord>()
            .FetchMany(x => x.Threads)
            .SingleOrDefault(x => x.Reference == patternReference, cancellationToken);

        await transaction.Commit(cancellationToken);

        if (pattern == null)
            return Result<PatternRecord>.Failure($"Unable to find pattern with reference: '{patternReference}'.");

        return pattern;
    }
}