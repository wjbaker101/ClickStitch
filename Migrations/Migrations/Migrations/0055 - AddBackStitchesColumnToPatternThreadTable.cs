using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0055)]
public sealed class AddBackStitchesColumnToPatternThreadTable_0055 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.PATTERN_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AddColumn("back_stitches")
            .AsCustom("json")
            .Nullable();
    }
}