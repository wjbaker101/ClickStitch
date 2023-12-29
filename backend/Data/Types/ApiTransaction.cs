using NHibernate;

namespace Data.Types;

public interface IApiTransaction : IDisposable
{
    Task Commit(CancellationToken cancellationToken);
}

public sealed class ApiTransaction : IApiTransaction
{
    private readonly ITransaction _transaction;

    public ApiTransaction(ITransaction transaction)
    {
        _transaction = transaction;
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        await _transaction.CommitAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction.Dispose();
    }
}