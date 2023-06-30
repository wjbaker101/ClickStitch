using Data.Repositories.UserPatternThreadStitch.Types;

namespace Data.Repositories.UserPatternThreadStitch;

public interface IUserPatternThreadStitchRepository : IRepository<UserPatternThreadStitchRecord>
{
    Task Complete(UserRecord user, Guid patternReference, StitchPosition positions, CancellationToken cancellationToken);
}

public sealed class UserPatternThreadStitchRepository : Repository<UserPatternThreadStitchRecord>, IUserPatternThreadStitchRepository
{
    public UserPatternThreadStitchRepository(IDatabase database) : base(database)
    {
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
            var stitchPositions = positions.StitchesByThread[thread.Index];

            var stitch = await session
                .Query<PatternThreadStitchRecord>()
                .Where(stitch => stitch.Thread == thread && stitchPositions.Any(pos => pos.X == stitch.X && pos.Y == stitch.Y))
                .SingleOrDefaultAsync(cancellationToken);

            await session.SaveAsync(new UserPatternThreadStitchRecord
            {
                User = user,
                Stitch = stitch,
                StitchedAt = DateTime.UtcNow
            }, cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
    }
}