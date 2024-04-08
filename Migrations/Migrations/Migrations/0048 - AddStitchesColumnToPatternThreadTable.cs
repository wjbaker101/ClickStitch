using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0048)]
public sealed class AddStitchesColumnToPatternThreadTable_0048 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.PATTERN_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AddColumn("stitches")
            .AsCustom("json")
            .Nullable();
    }
}