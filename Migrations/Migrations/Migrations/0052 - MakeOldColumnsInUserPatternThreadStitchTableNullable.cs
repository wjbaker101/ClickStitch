using FluentMigrator;
using Migrations.Common;
using Migrations.Extensions;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0052)]
public sealed class MakeOldColumnsInUserPatternThreadStitchTableNullable_0052 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AlterColumn("pattern_thread_stitch_id")
            .AsInt64()
            .Nullable()
            .AlterColumn("stitched_at")
            .AsTimestampWithTimeZone()
            .Nullable();
    }
}