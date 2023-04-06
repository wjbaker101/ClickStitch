using Core.Types;
using Data.Records;
using NHibernate.Linq;

namespace Data.Repositories.UserPatternStitch;

public interface IUserPatternStitchRepository : IRepository<UserPatternStitchRecord>
{
    Task<Result> DeleteByPositions(UserPatternRecord userPattern, List<(int posX, int posY)> positions);
    Task<Dictionary<long, UserPatternStitchRecord>> GetByUserPattern(UserPatternRecord userPattern);
    Task<Result> CompleteByPositions(PatternRecord pattern, UserPatternRecord userPattern, List<(int posX, int posY)> positions);
}

public sealed class UserPatternStitchRepository : Repository<UserPatternStitchRecord>, IUserPatternStitchRepository
{
    public UserPatternStitchRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Result> DeleteByPositions(UserPatternRecord userPattern, List<(int posX, int posY)> positions)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        foreach (var position in positions)
        {
            await session
                .Query<UserPatternStitchRecord>()
                .Where(x =>
                    x.UserPattern == userPattern &&
                    x.X == position.posX &&
                    x.Y == position.posY)
                .DeleteAsync();
        }

        await transaction.CommitAsync();

        return Result.Success();
    }

    public async Task<Dictionary<long, UserPatternStitchRecord>> GetByUserPattern(UserPatternRecord userPattern)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var userPatterns = session
            .Query<UserPatternStitchRecord>()
            .Where(x => x.UserPattern == userPattern)
            .ToDictionary(x => x.Stitch.Id, x => x);

        await transaction.CommitAsync();

        return userPatterns;
    }

    public async Task<Result> CompleteByPositions(PatternRecord pattern, UserPatternRecord userPattern, List<(int posX, int posY)> positions)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        foreach (var position in positions)
        {
            var stitch = await session
                .Query<PatternStitchRecord>()
                .SingleOrDefaultAsync(x =>
                    x.Pattern == pattern &&
                    x.X == position.posX &&
                    x.Y == position.posY);

            await session.SaveAsync(new UserPatternStitchRecord
            {
                UserPattern = userPattern,
                Stitch = stitch,
                StitchedAt = DateTime.UtcNow,
                X = stitch.X,
                Y = stitch.Y
            });
        }

        await transaction.CommitAsync();

        return Result.Success();
    }
}