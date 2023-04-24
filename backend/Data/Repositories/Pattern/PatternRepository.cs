using Core.Types;
using Data.Records;
using Data.Repositories.Pattern.Types;
using NHibernate.Linq;

namespace Data.Repositories.Pattern;

public interface IPatternRepository : IRepository<PatternRecord>
{
    Task<List<PatternRecord>> SearchAsync(SearchPatternsParameters parameters, CancellationToken cancellationToken);
    Task<Result<PatternRecord>> GetByReferenceAsync(Guid patternReference, CancellationToken cancellationToken);
    Task<Result<PatternRecord>> GetFullByReferenceAsync(Guid patternReference, CancellationToken cancellationToken);
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
            .SingleOrDefaultAsync(x => x.Reference == patternReference, cancellationToken);

        if (pattern == null)
            return Result<PatternRecord>.Failure($"Unable to find pattern with reference: '{patternReference}'.");

        await transaction.CommitAsync(cancellationToken);

        return pattern;
    }

    public async Task<Result<PatternRecord>> GetFullByReferenceAsync(Guid patternReference, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<PatternRecord>()
            .Where(x => x.Reference == patternReference);
        query
            .FetchMany(x => x.Threads)
            .ToFuture();
        query
            .FetchMany(x => x.Stitches)
            .ToFuture();

        var pattern = query
            .ToFuture()
            .SingleOrDefault();

        if (pattern == null)
            return Result<PatternRecord>.Failure($"Unable to find pattern with reference: '{patternReference}'.");

        await transaction.CommitAsync(cancellationToken);

        return pattern;
    }
}