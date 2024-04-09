using Data.Repositories.UserPatternThreadStitch.Types;
using Data.Types;

namespace Data.Repositories.UserPatternThreadStitch;

public interface IUserPatternThreadStitchRepository : IRepository<UserPatternThreadStitchRecord>
{
    Task<Dictionary<int, Dictionary<long, UserPatternThreadStitchRecord>>> GetByUser(UserRecord user, Guid patternReference, CancellationToken cancellationToken);
    Task<Dictionary<long, List<UserPatternThreadStitchRecord>>> GetByUserForThreads(UserRecord user, ISet<PatternThreadRecord> threads, CancellationToken cancellationToken);
    Task Complete(UserRecord user, Guid patternReference, StitchPosition positions, CancellationToken cancellationToken);
    Task UnComplete(UserRecord user, Guid patternReference, StitchPosition positions, CancellationToken cancellationToken);
}

public sealed class UserPatternThreadStitchRepository : Repository<UserPatternThreadStitchRecord>, IUserPatternThreadStitchRepository
{
    public UserPatternThreadStitchRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Dictionary<int, Dictionary<long, UserPatternThreadStitchRecord>>> GetByUser(UserRecord user, Guid patternReference, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var threads = await session
            .Query<PatternThreadRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.Pattern.Reference == patternReference)
            .ToList(cancellationToken);

        var userStitches = (await session
            .Query<UserPatternThreadStitchRecord>()
            .Fetch(x => x.Stitch)
            .ThenFetch(x => x.Thread)
            .Where(x => x.User == user && threads.Contains(x.Stitch.Thread))
            .ToList(cancellationToken))
            .GroupBy(x => x.Stitch.Thread.Index)
            .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Stitch.Id));

        await transaction.Commit(cancellationToken);

        return userStitches;
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
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var threadIndexes = positions.StitchesByThread.Select(x => x.Key).ToHashSet();

        var threads = await session
            .Query<PatternThreadRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.Pattern.Reference == patternReference && threadIndexes.Contains(x.Index))
            .ToList(cancellationToken);

        foreach (var thread in threads)
        {
            var stitchPositions = positions.StitchesByThread[thread.Index].ConvertAll(x => $"{x.X},{x.Y}");

            var stitches = await session
                .Query<PatternThreadStitchRecord>()
                .Where(stitch => stitch.Thread == thread && stitchPositions.Contains(stitch.LookupHash))
                .ToList(cancellationToken);

            foreach (var stitch in stitches)
            {
                await session.Save(new UserPatternThreadStitchRecord
                {
                    User = user,
                    Stitch = stitch,
                    StitchedAt = DateTime.UtcNow,
                    Thread = thread,
                    X = stitch.X,
                    Y = stitch.Y,
                    CompletedAt = DateTime.UtcNow
                }, cancellationToken);
            }
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