using Core.Services;
using Guid = System.Guid;

namespace TestHelpers.Fakes;

public sealed class FakeGuid : IGuid
{
    private readonly Guid _guid;

    private FakeGuid(Guid guid)
    {
        _guid = guid;
    }

    public static FakeGuid Default() => new(Guid.NewGuid());

    public static FakeGuid With(Guid guid) => new(guid);

    public Guid NewGuid()
    {
        return _guid;
    }
}