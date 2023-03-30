using Core.Services;

namespace TestHelpers.Fakes;

public sealed class FakeDateTimeService : IDateTimeService
{
    private readonly DateTime _now;

    private FakeDateTimeService(DateTime now)
    {
        _now = now;
    }

    public static FakeDateTimeService Default() => new(DateTime.UtcNow);

    public static FakeDateTimeService With(DateTime dateTime) => new(dateTime);

    public DateTime UtcNow()
    {
        return _now;
    }
}