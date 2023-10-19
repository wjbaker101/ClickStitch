namespace ClickStitch.Models;

public sealed class PermissionModel
{
    public required ApiPermissionType Type { get; init; }
    public required string Name { get; init; }
}

public enum ApiPermissionType
{
    Unknown = 0,
    Admin = 1,
    Creator = 2,
    Stitcher = 3
}