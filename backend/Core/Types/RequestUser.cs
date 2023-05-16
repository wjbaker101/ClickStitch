namespace Core.Types;

public sealed class RequestUser
{
    public required long Id { get; init; }
    public required Guid Reference { get; init; }
    public required List<RequestPermissionType> Permissions { get; init; }
}

public enum RequestPermissionType
{
    Unknown = 0,
    Admin = 1
}