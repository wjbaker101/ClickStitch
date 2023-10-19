using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0030)]
public sealed class AddIsPublicColumnToPatternTable_0030 : Migration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AddColumn("is_public")
            .AsBoolean()
            .Nullable();
    }

    public override void Down()
    {
        Delete
            .Column("is_public")
            .FromTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}