using Data.Records;

namespace Data.Repositories.Pattern;

public interface IPatternThreadRepository : IRepository<PatternThreadRecord>
{
}

public sealed class PatternThreadRepository : Repository<PatternThreadRecord>, IPatternThreadRepository
{
    public PatternThreadRepository(IDatabase database) : base(database)
    {
    }
}