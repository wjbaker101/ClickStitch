using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0049)]
public sealed class MakeStitchesColumnOnPatternThreadTableNotNullable_0049 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.PATTERN_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AlterColumn("stitches")
            .AsCustom("json")
            .NotNullable();
    }
}