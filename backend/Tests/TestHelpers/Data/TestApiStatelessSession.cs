using Data.Types;
using NHibernate;

namespace TestHelpers.Data;

public sealed class TestApiStatelessSession : IApiStatelessSession
{
    private readonly List<IDatabaseRecord> _records;
    private readonly DatabaseActionsListener _actions;

    public TestApiStatelessSession(List<IDatabaseRecord> records, DatabaseActionsListener actions)
    {
        _records = records;
        _actions = actions;
    }

    public Task<IApiTransaction> BeginTransaction(CancellationToken cancellationToken)
    {
        return Task.FromResult<IApiTransaction>(new TestApiTransaction());
    }

    public IApiQueryable<TRecord> Query<TRecord>() where TRecord : IDatabaseRecord
    {
        return new TestApiQueryable<TRecord>(_records.OfType<TRecord>().AsQueryable());
    }

    public Task<TRecord> Insert<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord
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

    public ISQLQuery CreateSqlQuery(string sql)
    {
        return new TestSqlQuery(sql);
    }

    public void Dispose()
    {
    }
}