using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0035)]
public sealed class RenamePositionColumnsInUserPatternTable_0035 : AutoReversingMigration
{
    public override void Up()
    {
        Rename
            .Column("last_position_x")
            .OnTable(Names.Tables.USER_PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .To("pause_position_x");

        Rename
            .Column("last_position_y")
            .OnTable(Names.Tables.USER_PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .To("pause_position_y");
    }
}