using Data.Types;

namespace Data.Repositories.Pattern;

public interface IPatternThreadStitchRepository : IRepository<PatternThreadStitchRecord>
{
}

public sealed class PatternThreadStitchRepository : Repository<PatternThreadStitchRecord>, IPatternThreadStitchRepository
{
    public PatternThreadStitchRepository(IDatabase database) : base(database)
    {
    }
}