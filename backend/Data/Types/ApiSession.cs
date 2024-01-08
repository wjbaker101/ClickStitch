using NHibernate;

namespace Data.Types;

public interface IApiSession : IDisposable
{
    Task<IApiTransaction> BeginTransaction(CancellationToken cancellationToken);
    Task<TRecord> Load<TRecord>(long id, CancellationToken cancellationToken) where TRecord : IDatabaseRecordWithId;
    IApiQueryable<TRecord> Query<TRecord>() where TRecord : IDatabaseRecord;
    Task<TRecord> Save<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord;
    Task<TRecord> Update<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord;
    Task Delete<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord;
}

public sealed class ApiSession : IApiSession
{
    private readonly ISession _session;

    public ApiSession(ISession session)
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

    public async Task<TRecord> Load<TRecord>(long id, CancellationToken cancellationToken) where TRecord : IDatabaseRecordWithId
    {
        return await _session.LoadAsync<TRecord>(id, cancellationToken);
    }

    public IApiQueryable<TRecord> Query<TRecord>() where TRecord : IDatabaseRecord
    {
        return new ApiQueryable<TRecord>(_session.Query<TRecord>());
    }

    public async Task<TRecord> Save<TRecord>(TRecord record, CancellationToken cancellationToken) where TRecord : IDatabaseRecord
    {
        await _session.SaveAsync(record, cancellationToken);
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

    public void Dispose()
    {
        _session.Dispose();
    }
}