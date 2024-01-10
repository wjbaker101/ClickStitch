using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0039)]
public sealed class UpdateAnchorThreadsToPadCode_0039 : Migration
{
    public override void Up()
    {
        Update
            .Table(Names.Tables.THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .Set(new { code = RawSql.Insert("'0' || code") })
            .Where(new { brand = "Anchor" });
    }

    public override void Down()
    {
    }
}