using Data.Records;

namespace ClickStitch.Models.Mappers;

public static class PermissionMapper
{
    public static PermissionModel Map(PermissionRecord permission) => new()
    {
        Type = MapType(permission.Type),
        Name = permission.Name
    };

    private static ApiPermissionType MapType(PermissionType type) => type switch
    {
        PermissionType.Admin => ApiPermissionType.Admin,
        PermissionType.Creator => ApiPermissionType.Creator,
        PermissionType.Stitcher => ApiPermissionType.Stitcher,

        PermissionType.Unknown or _ => throw new NotSupportedException($"Unable to map permission type: '{type}'.")
    };
}