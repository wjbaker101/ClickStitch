using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0012)]
public sealed class AddLastLoginAtColumnToUserTable_0012 : Migration
{
    public override void Up()
    {
        Create
            .Column("last_login_at")
            .OnTable(Names.Tables.USER)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AsDateTimeOffset()
            .Nullable();
    }

    public override void Down()
    {
        Delete
            .Column("last_login_at")
            .FromTable(Names.Tables.USER)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}