using Data.Types;

namespace TestHelpers.Data;

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