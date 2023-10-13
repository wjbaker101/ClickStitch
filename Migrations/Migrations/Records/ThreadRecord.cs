namespace Migrations.Records;

public static class ThreadRecord
{
    public static object Create(Guid reference, string brand, string code) => new
    {
        reference,
        brand,
        code
    };
}