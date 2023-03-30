﻿using Core.Types;
using Data.Records;
using NHibernate.Linq;

namespace Data.Repositories.UserPattern;

public interface IUserPatternRepository : IRepository<UserPatternRecord>
{
    List<UserPatternRecord> GetByUser(UserRecord user);
    Result<UserPatternRecord> GetByUserAndPattern(UserRecord user, PatternRecord pattern);
}

public sealed class UserPatternRepository : Repository<UserPatternRecord>, IUserPatternRepository
{
    public UserPatternRepository(IDatabase database) : base(database)
    {
    }

    public List<UserPatternRecord> GetByUser(UserRecord user)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var userPatterns = session
            .Query<UserPatternRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.User == user)
            .ToList();

        transaction.Commit();

        return userPatterns;
    }

    public Result<UserPatternRecord> GetByUserAndPattern(UserRecord user, PatternRecord pattern)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var userPattern = session
            .Query<UserPatternRecord>()
            .Fetch(x => x.Pattern)
            .SingleOrDefault(x => x.User == user && x.Pattern == pattern);

        if (userPattern == null)
            return Result<UserPatternRecord>.Failure("Unable to find pattern for user.");

        transaction.Commit();

        return userPattern;
    }
}