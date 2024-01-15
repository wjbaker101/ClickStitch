using Data;
using Data.Types;
using NHibernate;

namespace TestHelpers.Data;

public sealed class TestDatabase : IDatabase
{
    public List<IDatabaseRecord> Records { get; set; } = new();
    public DatabaseActionsListener Actions { get; } = new();

    public ISessionFactory SessionFactory { get; }

    public IApiSession OpenSession(bool shouldOutputSql = true)
    {
        return new TestApiSession(Records, Actions);
    }

    public IApiStatelessSession OpenStatelessSession()
    {
        return new TestApiStatelessSession(Records, Actions);
    }
}