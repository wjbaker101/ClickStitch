using FluentMigrator;
using Migrations.Common;
using Migrations.Extensions;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0056)]
public sealed class CreateUserPatternThreadBackStitchTable_0056 : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.USER_PATTERN_THREAD_BACK_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithIdColumn()
            .WithColumn("user_id")
            .AsInt64()
            .NotNullable()
            .WithColumn("pattern_thread_id")
            .AsInt64()
            .NotNullable()
            .WithColumn("start_x")
            .AsInt32()
            .NotNullable()
            .WithColumn("start_y")
            .AsInt32()
            .NotNullable()
            .WithColumn("end_x")
            .AsInt32()
            .NotNullable()
            .WithColumn("end_y")
            .AsInt32()
            .NotNullable()
            .WithColumn("completed_at")
            .AsTimestampWithTimeZone()
            .NotNullable();

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_PATTERN_THREAD_BACK_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("user_id")
            .ToTable(Names.Tables.USER)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_PATTERN_THREAD_BACK_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("pattern_thread_id")
            .ToTable(Names.Tables.PATTERN_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .Index()
            .OnTable(Names.Tables.USER_PATTERN_THREAD_BACK_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("user_id")
            .Ascending()
            .OnColumn("pattern_thread_id")
            .Ascending();
    }
}