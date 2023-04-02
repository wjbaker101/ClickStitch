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
}