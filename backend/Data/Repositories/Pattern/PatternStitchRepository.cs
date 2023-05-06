namespace Data.Repositories.Pattern;

public interface IPatternStitchRepository : IRepository<PatternStitchRecord>
{
    Task<Result> SaveStitches(List<PatternStitchRecord> records, CancellationToken cancellationToken);
    Task<Result<PatternStitchRecord>> GetByPosition(PatternRecord pattern, int posX, int posY, CancellationToken cancellationToken);
}

public sealed class PatternStitchRepository : Repository<PatternStitchRecord>, IPatternStitchRepository
{
    public PatternStitchRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Result> SaveStitches(List<PatternStitchRecord> records, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenStatelessSession();
        using var transaction = session.BeginTransaction();

        for (var index = 0; index < records.Count; ++index)
            await session.InsertAsync(records[index], cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<PatternStitchRecord>> GetByPosition(PatternRecord pattern, int posX, int posY, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var stitch = await session
            .Query<PatternStitchRecord>()
            .SingleOrDefaultAsync(x =>
                x.Pattern == pattern &&
                x.X == posX &&
                x.Y == posY, cancellationToken);

        if (stitch == null)
            return Result<PatternStitchRecord>.Failure($"Unable to find stitch at x:{posX} y:{posY}.");

        await transaction.CommitAsync(cancellationToken);

        return stitch;
    }
}