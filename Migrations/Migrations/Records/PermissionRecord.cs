namespace Migrations.Records;

public static class PermissionRecord
{
    public static object Create(int type, string name) => new
    {
        type,
        name
    };
}