using Data.Repositories.UserPatternThreadBackStitch.Types;
using Data.Types;

namespace Data.Repositories.UserPatternThreadBackStitch;

public interface IUserPatternThreadBackStitchRepository : IRepository<UserPatternThreadBackStitchRecord>
{
    Task<Dictionary<long, List<UserPatternThreadBackStitchRecord>>> GetByUserForThreads(UserRecord user, ISet<PatternThreadRecord> threads, CancellationToken cancellationToken);
    Task Complete(UserRecord user, Guid patternReference, BackStitchPositions positions, CancellationToken cancellationToken);
    Task UnComplete(UserRecord user, Guid patternReference, BackStitchPositions positions, CancellationToken cancellationToken);
}

public sealed class UserPatternThreadBackStitchRepository : Repository<UserPatternThreadBackStitchRecord>, IUserPatternThreadBackStitchRepository
{
    public UserPatternThreadBackStitchRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Dictionary<long, List<UserPatternThreadBackStitchRecord>>> GetByUserForThreads(UserRecord user, ISet<PatternThreadRecord> threads, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var userStitches = (await session
            .Query<UserPatternThreadBackStitchRecord>()
            .Where(x => x.User == user && threads.Contains(x.Thread))
            .ToList(cancellationToken))
            .GroupBy(x => x.Thread.Id)
            .ToDictionary(x => x.Key, x => x.ToList());

        await transaction.Commit(cancellationToken);

        return userStitches;
    }

    public async Task Complete(UserRecord user, Guid patternReference, BackStitchPositions positions, CancellationToken cancellationToken)
    {
        using var session = Database.OpenStatelessSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var threadIndexes = positions.StitchesByThread.Select(x => x.Key).ToHashSet();

        var threads = await session
            .Query<PatternThreadRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.Pattern.Reference == patternReference && threadIndexes.Contains(x.Index))
            .ToList(cancellationToken);

        foreach (var thread in threads)
        {
            var stitches = positions.StitchesByThread[thread.Index];

            var inserts = string.Join(',', stitches.ConvertAll(x => $"({user.Id},{thread.Id},{x.StartX},{x.StartY},{x.EndX},{x.EndY},'{DateTime.UtcNow}')"));

            await session
                .CreateSqlQuery($"insert into clickstitch.user_pattern_thread_back_stitch (user_id,pattern_thread_id,start_x,start_y,end_x,end_y,completed_at) values {inserts};")
                .ExecuteUpdateAsync(cancellationToken);
        }

        await transaction.Commit(cancellationToken);
    }

    public async Task UnComplete(UserRecord user, Guid patternReference, BackStitchPositions positions, CancellationToken cancellationToken)
    {
        using var session = Database.OpenStatelessSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var threadIndexes = positions.StitchesByThread.Select(x => x.Key).ToHashSet();

        var threads = await session
            .Query<PatternThreadRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.Pattern.Reference == patternReference && threadIndexes.Contains(x.Index))
            .ToList(cancellationToken);

        foreach (var thread in threads)
        {
            var stitchPositions = positions.StitchesByThread[thread.Index];

            await session
                .CreateSqlQuery($"""
                    delete from clickstitch.user_pattern_thread_back_stitch
                    where user_id=:userId
                    and pattern_thread_id=:patternThreadId
                    and ({string.Join(" or ", stitchPositions.ConvertAll(x => $"start_x={x.StartX} and start_y={x.StartY} and end_x={x.EndX} and end_y={x.EndY}"))});
                    """)
                .SetParameter("userId", user.Id)
                .SetParameter("patternThreadId", thread.Id)
                .ExecuteUpdateAsync(cancellationToken);
        }

        await transaction.Commit(cancellationToken);
    }
}