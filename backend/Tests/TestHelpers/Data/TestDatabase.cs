using Data;
using Data.Types;

namespace TestHelpers.Data;

public sealed class TestDatabase : IDatabase
{
    public List<IDatabaseRecord> Records { get; set; } = new();
    public DatabaseActionsListener Actions { get; } = new();

    public IApiSession OpenSession()
    {
        return new TestApiSession(Records, Actions);
    }

    public IApiStatelessSession OpenStatelessSession()
    {
        return new TestApiStatelessSession(Records, Actions);
    }
}