using Core.Services;

namespace TestHelpers.Fakes;

public sealed class FakeGuidService : IGuidService
{
    private readonly Guid _guid;

    private FakeGuidService(Guid guid)
    {
        _guid = guid;
    }

    public static FakeGuidService Default() => new(Guid.NewGuid());

    public static FakeGuidService With(Guid guid) => new(guid);

    public Guid NewGuid()
    {
        return _guid;
    }
}