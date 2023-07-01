using Data.Repositories.UserPatternThreadStitch.Types;

namespace Data.Repositories.UserPatternThreadStitch;

public interface IUserPatternThreadStitchRepository : IRepository<UserPatternThreadStitchRecord>
{
    Task<Dictionary<int, Dictionary<long, UserPatternThreadStitchRecord>>> GetByUser(UserRecord user, Guid patternReference, CancellationToken cancellationToken);
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
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var threads = await session
            .Query<PatternThreadRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.Pattern.Reference == patternReference)
            .ToListAsync(cancellationToken);

        var userStitches = (await session
            .Query<UserPatternThreadStitchRecord>()
            .Fetch(x => x.Stitch)
            .ThenFetch(x => x.Thread)
            .Where(x => x.User == user && threads.Contains(x.Stitch.Thread))
            .ToListAsync(cancellationToken))
            .GroupBy(x => x.Stitch.Thread.Index)
            .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Stitch.Id));

        await transaction.CommitAsync(cancellationToken);

        return userStitches;
    }

    public async Task Complete(UserRecord user, Guid patternReference, StitchPosition positions, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var threadIndexes = positions.StitchesByThread.Select(x => x.Key).ToHashSet();

        var threads = await session
            .Query<PatternThreadRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.Pattern.Reference == patternReference && threadIndexes.Contains(x.Index))
            .ToListAsync(cancellationToken);

        foreach (var thread in threads)
        {
            var stitchPositions = positions.StitchesByThread[thread.Index].ConvertAll(x => x.X.ToString() + x.Y);

            var stitches = await session
                .Query<PatternThreadStitchRecord>()
                .Where(stitch => stitch.Thread == thread && stitchPositions.Contains(stitch.X.ToString() + stitch.Y.ToString()))
                .ToListAsync(cancellationToken);

            foreach (var stitch in stitches)
            {
                await session.SaveAsync(new UserPatternThreadStitchRecord
                {
                    User = user,
                    Stitch = stitch,
                    StitchedAt = DateTime.UtcNow
                }, cancellationToken);
            }
        }

        await transaction.CommitAsync(cancellationToken);
    }

    public async Task UnComplete(UserRecord user, Guid patternReference, StitchPosition positions, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var threadIndexes = positions.StitchesByThread.Select(x => x.Key).ToHashSet();

        var threads = await session
            .Query<PatternThreadRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.Pattern.Reference == patternReference && threadIndexes.Contains(x.Index))
            .ToListAsync(cancellationToken);

        foreach (var thread in threads)
        {
            var stitchPositions = positions.StitchesByThread[thread.Index].ConvertAll(x => x.X.ToString() + x.Y);

            await session
                .Query<UserPatternThreadStitchRecord>()
                .Fetch(x => x.Stitch)
                .Where(stitch => stitch.Stitch.Thread == thread && stitchPositions.Contains(stitch.Stitch.X.ToString() + stitch.Stitch.Y.ToString()))
                .DeleteAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
    }
}