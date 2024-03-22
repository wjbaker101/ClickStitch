using NHibernate;

namespace Data.Types;

public interface IApiStatelessSession : IDisposable
{
    Task<IApiTransaction> BeginTransaction(CancellationToken cancellationToken);
    IApiQueryable<TRecord> Query<TRecord>() where TRecord : IDatabaseRecord;
    Task<TRecord> Insert<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord;
    Task<TRecord> Update<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord;
    Task Delete<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord;
    ISQLQuery CreateSqlQuery(string sql);
}

public sealed class ApiStatelessSession : IApiStatelessSession
{
    private readonly IStatelessSession _session;

    public ApiStatelessSession(IStatelessSession session)
    {
        _session = session;
    }

    public async Task<IApiTransaction> BeginTransaction(CancellationToken cancellationToken)
    {
        await _session
            .GetSessionImplementation()
            .ConnectionManager
            .GetConnectionAsync(cancellationToken)
            .ConfigureAwait(false);

        return new ApiTransaction(_session.BeginTransaction());
    }

    public IApiQueryable<TRecord> Query<TRecord>() where TRecord : IDatabaseRecord
    {
        return new ApiQueryable<TRecord>(_session.Query<TRecord>());
    }

    public async Task<TRecord> Insert<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord
    {
        await _session.InsertAsync(record, cancellationToken);
        return record;
    }

    public async Task<TRecord> Update<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord
    {
        await _session.UpdateAsync(record, cancellationToken);
        return record;
    }

    public async Task Delete<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord
    {
        await _session.DeleteAsync(record, cancellationToken);
    }

    public ISQLQuery CreateSqlQuery(string sql)
    {
        return _session.CreateSQLQuery(sql);
    }

    public void Dispose()
    {
        _session.Dispose();
    }
}