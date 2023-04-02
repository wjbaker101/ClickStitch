namespace Core.Services;

public interface IDateTime
{
    DateTime UtcNow();
}

public sealed class DateTimeProvider : IDateTime
{
    public DateTime UtcNow() => DateTime.UtcNow;
}