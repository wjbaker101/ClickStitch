namespace Migrations.Records;

public static class ThreadRecord
{
    public static object Create1(Guid reference, string brand, string code) => new
    {
        reference,
        brand,
        code
    };

    public static object Create2(Guid reference, string brand, string code, string colour = null!) => new
    {
        reference,
        brand,
        code,
        colour
    };
}