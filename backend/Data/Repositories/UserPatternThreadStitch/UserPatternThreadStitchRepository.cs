﻿using Data.Repositories.UserPatternThreadStitch.Types;
using Data.Types;

namespace Data.Repositories.UserPatternThreadStitch;

public interface IUserPatternThreadStitchRepository : IRepository<UserPatternThreadStitchRecord>
{
    Task<Dictionary<long, List<UserPatternThreadStitchRecord>>> GetByUserForThreads(UserRecord user, ISet<PatternThreadRecord> threads, CancellationToken cancellationToken);
    Task Complete(UserRecord user, Guid patternReference, StitchPosition positions, CancellationToken cancellationToken);
    Task UnComplete(UserRecord user, Guid patternReference, StitchPosition positions, CancellationToken cancellationToken);
}

public sealed class UserPatternThreadStitchRepository : Repository<UserPatternThreadStitchRecord>, IUserPatternThreadStitchRepository
{
    public UserPatternThreadStitchRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Dictionary<long, List<UserPatternThreadStitchRecord>>> GetByUserForThreads(UserRecord user, ISet<PatternThreadRecord> threads, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var userStitches = (await session
            .Query<UserPatternThreadStitchRecord>()
            .Where(x => x.User == user && threads.Contains(x.Thread))
            .ToList(cancellationToken))
            .GroupBy(x => x.Thread.Id)
            .ToDictionary(x => x.Key, x => x.ToList());

        await transaction.Commit(cancellationToken);

        return userStitches;
    }

    public async Task Complete(UserRecord user, Guid patternReference, StitchPosition positions, CancellationToken cancellationToken)
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

            var inserts = string.Join(',', stitches.ConvertAll(x => $"({user.Id},{thread.Id},{x.X},{x.Y},'{DateTime.UtcNow}')"));

            await session
                .CreateSqlQuery($"insert into clickstitch.user_pattern_thread_stitch (user_id,pattern_thread_id,x,y,completed_at) values {inserts};")
                .ExecuteUpdateAsync(cancellationToken);
        }

        await transaction.Commit(cancellationToken); 
    }

    public async Task UnComplete(UserRecord user, Guid patternReference, StitchPosition positions, CancellationToken cancellationToken)
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
                .CreateSqlQuery($"delete from clickstitch.user_pattern_thread_stitch where user_id=:userId and pattern_thread_id=:patternThreadId and ({string.Join(" or ", stitchPositions.ConvertAll(x => $"x={x.X} and y={x.Y}"))});")
                .SetParameter("userId", user.Id)
                .SetParameter("patternThreadId", thread.Id)
                .ExecuteUpdateAsync(cancellationToken);
        }

        await transaction.Commit(cancellationToken);
    }
}