using NHibernate;

namespace Data.Types;

public interface IApiSession : IDisposable
{
    Task<IApiTransaction> BeginTransaction(CancellationToken cancellationToken);
    IQueryable<TRecord> Query<TRecord>() where TRecord : IDatabaseRecord;
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

    public IQueryable<TRecord> Query<TRecord>() where TRecord : IDatabaseRecord
    {
        return _session.Query<TRecord>();
    }

    public void Dispose()
    {
        _session.Dispose();
    }
}