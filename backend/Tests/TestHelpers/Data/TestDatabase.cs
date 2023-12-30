using Data;
using Data.Types;
using NHibernate;

namespace TestHelpers.Data;

public sealed class TestDatabase : IDatabase
{
    public List<IDatabaseRecord> Records { get; set; } = new();
    public DatabaseActionsListener Actions { get; } = new();

    public ISessionFactory SessionFactory { get; }

    public IApiSession OpenSession()
    {
        return new TestApiSession(Records, Actions);
    }
}

public sealed class TestApiSession : IApiSession
{
    private readonly List<IDatabaseRecord> _records;
    private readonly DatabaseActionsListener _actions;

    public TestApiSession(List<IDatabaseRecord> records, DatabaseActionsListener actions)
    {
        _records = records;
        _actions = actions;
    }

    public Task<IApiTransaction> BeginTransaction(CancellationToken cancellationToken)
    {
        return Task.FromResult<IApiTransaction>(new TestApiTransaction());
    }

    public Task<TRecord> Load<TRecord>(long id, CancellationToken cancellationToken) where TRecord : IDatabaseRecord
    {
        return Task.FromResult(_records.OfType<TRecord>().First());
    }

    public IApiQueryable<TRecord> Query<TRecord>() where TRecord : IDatabaseRecord
    {
        return new TestApiQueryable<TRecord>(_records.OfType<TRecord>().AsQueryable());
    }

    public Task<TRecord> Save<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord
    {
        _records.Add(record);
        _actions.OnSave(record);

        return Task.FromResult(record);
    }

    public Task<TRecord> Update<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord
    {
        _actions.OnUpdate(record);
        return Task.FromResult(record);
    }

    public Task Delete<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord
    {
        _records.Remove(record);
        _actions.OnDelete(record);

        return Task.FromResult(record);
    }

    public void Dispose()
    {
    }
}

public sealed class TestApiTransaction : IApiTransaction
{
    public Task Commit(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}