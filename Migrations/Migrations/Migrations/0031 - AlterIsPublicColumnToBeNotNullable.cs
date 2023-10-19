using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0031)]
public sealed class AlterIsPublicColumnToBeNotNullable_0031 : Migration
{
    public override void Up()
    {
        Alter
            .Column("is_public")
            .OnTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AsBoolean()
            .NotNullable();
    }

    public override void Down()
    {
        Alter
            .Column("is_public")
            .OnTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AsBoolean()
            .Nullable();
    }
}