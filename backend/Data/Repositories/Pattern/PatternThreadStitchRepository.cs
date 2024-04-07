using Data.Types;

namespace Data.Repositories.Pattern;

public interface IPatternThreadStitchRepository : IRepository<PatternThreadStitchRecord>
{
    Task SaveAll(List<PatternThreadStitchRecord> stitches, CancellationToken cancellationToken);
    Task DeleteByThreads(ISet<PatternThreadRecord> threads, CancellationToken cancellationToken);
}

public sealed class PatternThreadStitchRepository : Repository<PatternThreadStitchRecord>, IPatternThreadStitchRepository
{
    public PatternThreadStitchRepository(IDatabase database) : base(database)
    {
    }

    public async Task SaveAll(List<PatternThreadStitchRecord> stitches, CancellationToken cancellationToken)
    {
        var inserts = string.Join(',', stitches.ConvertAll(x => $"({x.Thread.Id},{x.X},{x.Y},'{x.LookupHash}')"));
        var sql = $"insert into clickstitch.pattern_thread_stitch (pattern_thread_id,x,y,lookup_hash) values {inserts};";

        using var session = Database.OpenStatelessSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        await session
            .CreateSqlQuery(sql)
            .ExecuteUpdateAsync(cancellationToken);

        await transaction.Commit(cancellationToken);
    }

    public async Task DeleteByThreads(ISet<PatternThreadRecord> threads, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        await session
            .Query<PatternThreadStitchRecord>()
            .Where(x => threads.Contains(x.Thread))
            .Delete(cancellationToken);

        await transaction.Commit(cancellationToken);
    }
}