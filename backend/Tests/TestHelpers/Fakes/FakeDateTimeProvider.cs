using Core.Services;
using DateTime = System.DateTime;

namespace TestHelpers.Fakes;

public sealed class FakeDateTime : IDateTime
{
    private readonly DateTime _now;

    private FakeDateTime(DateTime now)
    {
        _now = now;
    }

    public static FakeDateTime Default() => new(DateTime.UtcNow);

    public static FakeDateTime With(DateTime dateTime) => new(dateTime);

    public DateTime UtcNow()
    {
        return _now;
    }
}