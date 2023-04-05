namespace Core.Types;

public sealed class RequestUser
{
    public required long Id { get; init; }
    public required Guid Reference { get; init; }
}