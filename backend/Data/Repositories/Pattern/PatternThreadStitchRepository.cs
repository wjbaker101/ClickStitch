using Data.Types;

namespace Data.Repositories.Pattern;

public interface IPatternThreadStitchRepository : IRepository<PatternThreadStitchRecord>
{
    Task SaveAll(List<PatternThreadStitchRecord> stitches, CancellationToken cancellationToken);
}

public sealed class PatternThreadStitchRepository : Repository<PatternThreadStitchRecord>, IPatternThreadStitchRepository
{
    public PatternThreadStitchRepository(IDatabase database) : base(database)
    {
    }

    public async Task SaveAll(List<PatternThreadStitchRecord> stitches, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession(shouldOutputSql: false);
        using var transaction = await session.BeginTransaction(cancellationToken);

        foreach (var stitch in stitches)
            await session.Save(stitch, cancellationToken);

        await transaction.Commit(cancellationToken);
    }
}