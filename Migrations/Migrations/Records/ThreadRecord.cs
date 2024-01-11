namespace Migrations.Records;

public static class ThreadRecord
{
    public static object Create(Guid reference, string brand, string code, string colour = null!) => new
    {
        reference,
        brand,
        code,
        colour
    };
}