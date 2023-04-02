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

    public static FakeDateTime Default() => new(new DateTime(2020, 01, 02, 23, 24, 25));

    public static FakeDateTime With(DateTime dateTime) => new(dateTime);

    public DateTime UtcNow()
    {
        return _now;
    }
}