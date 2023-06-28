using FluentMigrator;
using Migrations.Common;
using Migrations.Extensions;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0019)]
public sealed class CreateUserPatternThreadStitchTable_0019 : Migration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithIdColumn()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("pattern_thread_stitch_id").AsInt64().NotNullable()
            .WithColumn("stitched_at").AsTimestampWithTimeZone().NotNullable();

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("user_id")
            .ToTable(Names.Tables.USER)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("pattern_thread_stitch_id")
            .ToTable(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .Index()
            .OnTable(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("user_id").Ascending()
            .OnColumn("pattern_thread_stitch_id").Ascending()
            .WithOptions()
            .Unique();
    }

    public override void Down()
    {
        Delete
            .Table(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}