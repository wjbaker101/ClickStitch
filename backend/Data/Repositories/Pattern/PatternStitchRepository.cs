using Core.Types;
using Data.Records;
using NHibernate.Linq;

namespace Data.Repositories.Pattern;

public interface IPatternStitchRepository : IRepository<PatternStitchRecord>
{
    Task<Result<PatternStitchRecord>> GetByPosition(PatternRecord pattern, int posX, int posY);
}

public sealed class PatternStitchRepository : Repository<PatternStitchRecord>, IPatternStitchRepository
{
    public PatternStitchRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Result<PatternStitchRecord>> GetByPosition(PatternRecord pattern, int posX, int posY)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var stitch = await session
            .Query<PatternStitchRecord>()
            .SingleOrDefaultAsync(x =>
                x.Pattern == pattern &&
                x.X == posX &&
                x.Y == posY);

        if (stitch == null)
            return Result<PatternStitchRecord>.Failure($"Unable to find stitch at x:{posX} y:{posY}.");

        await transaction.CommitAsync();

        return stitch;
    }
}