using FluentMigrator;
using Migrations.Common;
using Migrations.Extensions;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0051)]
public sealed class MakeNewPatternThreadStitchColumnsNotNullable_0051 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AlterColumn("pattern_thread_id")
            .AsInt64()
            .NotNullable()
            .AlterColumn("x")
            .AsInt32()
            .NotNullable()
            .AlterColumn("y")
            .AsInt32()
            .NotNullable()
            .AlterColumn("completed_at")
            .AsTimestampWithTimeZone()
            .NotNullable(); 
    }
}