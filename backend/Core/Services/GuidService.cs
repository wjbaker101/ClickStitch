namespace Core.Services;

public interface IGuidService
{
    Guid NewGuid();
}

public sealed class GuidService : IGuidService
{
    public Guid NewGuid() => Guid.NewGuid();
}