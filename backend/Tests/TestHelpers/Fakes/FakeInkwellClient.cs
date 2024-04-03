using Inkwell.Client;
using Inkwell.Client.Types;

namespace TestHelpers.Fakes;

public sealed class FakeInkwellClient : IInkwellClient
{
    public Task Log(CreateLogRequest request)
    {
        return Task.CompletedTask;
    }
}