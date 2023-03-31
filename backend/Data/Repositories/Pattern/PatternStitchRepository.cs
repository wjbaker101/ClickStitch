using Data.Records;

namespace Data.Repositories.Pattern;

public interface IPatternStitchRepository : IRepository<PatternStitchRecord>
{
}

public sealed class PatternStitchRepository : Repository<PatternStitchRecord>, IPatternStitchRepository
{
    public PatternStitchRepository(IDatabase database) : base(database)
    {
    }
}