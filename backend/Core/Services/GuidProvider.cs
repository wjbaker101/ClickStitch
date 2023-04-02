namespace Core.Services;

public interface IGuid
{
    Guid NewGuid();
}

public sealed class GuidProvider : IGuid
{
    public Guid NewGuid() => Guid.NewGuid();
}