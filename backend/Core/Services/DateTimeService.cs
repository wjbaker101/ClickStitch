namespace Core.Services;

public interface IDateTimeService
{
    DateTime UtcNow();
}

public sealed class DateTimeService : IDateTimeService
{
    public DateTime UtcNow() => DateTime.UtcNow;
}