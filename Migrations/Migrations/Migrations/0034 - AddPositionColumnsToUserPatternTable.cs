using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0034)]
public sealed class AddPositionColumnsToUserPatternTable_0034 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.USER_PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AddColumn("last_position_x")
            .AsInt32().Nullable()
            .AddColumn("last_position_y")
            .AsInt32().Nullable();
    }
}