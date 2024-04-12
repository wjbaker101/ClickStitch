using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0053)]
public sealed class DropOldColumnsInUserPatternThreadStitchTable_0053 : Migration
{
    public override void Up()
    {
        Delete
            .Column("pattern_thread_stitch_id")
            .FromTable(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH);

        Delete
            .Column("stitched_at")
            .FromTable(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }

    public override void Down()
    {
    }
}