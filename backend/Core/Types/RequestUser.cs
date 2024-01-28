namespace Core.Types;

public sealed class RequestUser
{
    public required long Id { get; init; }
    public required Guid Reference { get; init; }

    public RequestUserPermissions Permissions { get; }

    public RequestUser(List<RequestPermissionType> permissions)
    {
        Permissions = new RequestUserPermissions(permissions);
    }
}

public sealed class RequestUserPermissions : List<RequestPermissionType>
{
    public RequestUserPermissions(List<RequestPermissionType> permissions) : base(permissions)
    {
    }

    public bool Has(RequestPermissionType permission)
    {
        return this.Any(x => x == permission);
    }

    public bool IsAdmin() => Has(RequestPermissionType.Admin);
    public bool IsCreator() => Has(RequestPermissionType.Creator);
}

public enum RequestPermissionType
{
    Unknown = 0,
    Admin = 1,
    Creator = 2
}