using Core.Types;
using Data.Records;
using Data.Repositories.Pattern.Types;
using NHibernate.Linq;

namespace Data.Repositories.Pattern;

public interface IPatternRepository : IRepository<PatternRecord>
{
    List<PatternRecord> Search(SearchPatternsParameters parameters);
    Result<PatternRecord> GetByReference(Guid patternReference);
    Result<PatternRecord> GetFullByReference(Guid patternReference);
    Task<List<PatternRecord>> SearchAsync(SearchPatternsParameters parameters);
    Task<Result<PatternRecord>> GetByReferenceAsync(Guid patternReference);
    Task<Result<PatternRecord>> GetFullByReferenceAsync(Guid patternReference);
}

public sealed class PatternRepository : Repository<PatternRecord>, IPatternRepository
{
    public PatternRepository(IDatabase database) : base(database)
    {
    }

    public List<PatternRecord> Search(SearchPatternsParameters parameters)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var patterns = session
            .Query<PatternRecord>()
            .Where(x => !parameters.PatternFilter.Contains(x))
            .ToList();

        transaction.Commit();

        return patterns;
    }

    public Result<PatternRecord> GetByReference(Guid patternReference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var pattern = session
            .Query<PatternRecord>()
            .SingleOrDefault(x => x.Reference == patternReference);

        if (pattern == null)
            return Result<PatternRecord>.Failure($"Unable to find pattern with reference: '{patternReference}'.");

        transaction.Commit();

        return pattern;
    }

    public Result<PatternRecord> GetFullByReference(Guid patternReference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<PatternRecord>()
            .Where(x => x.Reference == patternReference);
        query
            .FetchMany(x => x.Stitches)
            .ToFuture();
        query
            .FetchMany(x => x.Threads)
            .ToFuture();

        var pattern = query
            .ToFuture()
            .SingleOrDefault();

        if (pattern == null)
            return Result<PatternRecord>.Failure($"Unable to find pattern with reference: '{patternReference}'.");

        transaction.Commit();

        return pattern;
    }

    public async Task<List<PatternRecord>> SearchAsync(SearchPatternsParameters parameters)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var patterns = await session
            .Query<PatternRecord>()
            .Where(x => !parameters.PatternFilter.Contains(x))
            .ToListAsync();

        await transaction.CommitAsync();

        return patterns;
    }

    public async Task<Result<PatternRecord>> GetByReferenceAsync(Guid patternReference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var pattern = await session
            .Query<PatternRecord>()
            .SingleOrDefaultAsync(x => x.Reference == patternReference);

        if (pattern == null)
            return Result<PatternRecord>.Failure($"Unable to find pattern with reference: '{patternReference}'.");

        await transaction.CommitAsync();

        return pattern;
    }

    public async Task<Result<PatternRecord>> GetFullByReferenceAsync(Guid patternReference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<PatternRecord>()
            .Where(x => x.Reference == patternReference);
        query
            .FetchMany(x => x.Stitches)
            .ToFuture();
        query
            .FetchMany(x => x.Threads)
            .ToFuture();

        var pattern = await query
            .SingleOrDefaultAsync();

        if (pattern == null)
            return Result<PatternRecord>.Failure($"Unable to find pattern with reference: '{patternReference}'.");

        await transaction.CommitAsync();

        return pattern;
    }
}