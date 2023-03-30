using Core.Types;
using Data.Records;
using Data.Repositories.Pattern.Types;

namespace Data.Repositories.Pattern;

public interface IPatternRepository : IRepository<PatternRecord>
{
    List<PatternRecord> Search(SearchPatternsParameters parameters);
    Result<PatternRecord> GetByReference(Guid patternReference);
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
}