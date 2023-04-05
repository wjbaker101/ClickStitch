﻿using Core.Types;
using Data.Records;
using NHibernate.Linq;

namespace Data.Repositories.UserPatternStitch;

public interface IUserPatternStitchRepository : IRepository<UserPatternStitchRecord>
{
    Task<Result> DeleteByPositionLookup(string positionLookup);
    Task<Dictionary<long, UserPatternStitchRecord>> GetByUserPattern(UserPatternRecord userPattern);
}

public sealed class UserPatternStitchRepository : Repository<UserPatternStitchRecord>, IUserPatternStitchRepository
{
    public UserPatternStitchRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Result> DeleteByPositionLookup(string positionLookup)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session
            .Query<UserPatternStitchRecord>()
            .Where(x => x.PositionLookup == positionLookup)
            .DeleteAsync();

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
}