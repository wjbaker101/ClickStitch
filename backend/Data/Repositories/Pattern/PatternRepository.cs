using Data.Repositories.Pattern.Types;
using Data.Types;

namespace Data.Repositories.Pattern;

public interface IPatternRepository : IRepository<PatternRecord>
{
    Task<List<PatternRecord>> SearchAsync(SearchPatternsParameters parameters, CancellationToken cancellationToken);
    Task<Result<PatternRecord>> GetByReferenceAsync(Guid patternReference, CancellationToken cancellationToken);
    Task<Result<PatternRecord>> GetWithThreadsByReferenceAsync(Guid patternReference, CancellationToken cancellationToken);
    Task<Dictionary<int, List<PatternThreadStitchRecord>>> GetStitchesByThreads(List<PatternThreadRecord> threads, CancellationToken cancellationToken);
}

public sealed class PatternRepository : Repository<PatternRecord>, IPatternRepository
{
    public PatternRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<PatternRecord>> SearchAsync(SearchPatternsParameters parameters, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var patterns = await session
            .Query<PatternRecord>()
            .Fetch(x => x.User)
            .ThenFetch(x => x.UserCreator)
            .ThenFetch(x => x.Creator)
            .Where(x => !parameters.PatternsToExclude.Contains(x))
            .ToListAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return patterns;
    }

    public async Task<Result<PatternRecord>> GetByReferenceAsync(Guid patternReference, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var pattern = await session
            .Query<PatternRecord>()
            .Fetch(x => x.User)
            .SingleOrDefaultAsync(x => x.Reference == patternReference, cancellationToken);

        if (pattern == null)
            return Result<PatternRecord>.Failure($"Unable to find pattern with reference: '{patternReference}'.");

        await transaction.CommitAsync(cancellationToken);

        return pattern;
    }

    public async Task<Result<PatternRecord>> GetWithThreadsByReferenceAsync(Guid patternReference, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var pattern = await session
            .Query<PatternRecord>()
            .Fetch(x => x.Threads)
            .SingleOrDefaultAsync(x => x.Reference == patternReference, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        if (pattern == null)
            return Result<PatternRecord>.Failure($"Unable to find pattern with reference: '{patternReference}'.");

        return pattern;
    }

    public async Task<Dictionary<int, List<PatternThreadStitchRecord>>> GetStitchesByThreads(List<PatternThreadRecord> threads, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var stitches = (await session
            .Query<PatternThreadStitchRecord>()
            .Where(x => threads.Contains(x.Thread))
            .ToListAsync(cancellationToken))
            .GroupBy(x => x.Thread.Index)
            .ToDictionary(x => x.Key, x => x.ToList());

        await transaction.CommitAsync(cancellationToken);

        return stitches;
    }
}