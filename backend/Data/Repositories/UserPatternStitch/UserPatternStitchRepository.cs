﻿using Data.Records;
using NHibernate.Linq;

namespace Data.Repositories.UserPatternStitch;

public interface IUserPatternStitchRepository : IRepository<UserPatternStitchRecord>
{
    Task<List<UserPatternStitchRecord>> GetByUserPattern(UserPatternRecord userPattern);
}

public sealed class UserPatternStitchRepository : Repository<UserPatternStitchRecord>, IUserPatternStitchRepository
{
    public UserPatternStitchRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<UserPatternStitchRecord>> GetByUserPattern(UserPatternRecord userPattern)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var userPatterns = await session
            .Query<UserPatternStitchRecord>()
            .Fetch(x => x.PatternStitch)
            .Where(x => x.UserPattern == userPattern.Id)
            .ToListAsync();

        await transaction.CommitAsync();

        return userPatterns;
    }
}