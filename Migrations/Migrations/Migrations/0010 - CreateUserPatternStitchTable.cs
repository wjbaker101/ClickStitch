using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0010)]
public sealed class CreateUserPatternStitchTable_0010 : Migration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.USER_PATTERN_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("user_pattern_id").AsInt64().NotNullable()
            .WithColumn("pattern_stitch_id").AsInt64().NotNullable()
            .WithColumn("stitched_at").AsDateTimeOffset().NotNullable()
            .WithColumn("x").AsInt32().NotNullable()
            .WithColumn("y").AsInt32().NotNullable();

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_PATTERN_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("user_pattern_id")
            .ToTable(Names.Tables.USER_PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_PATTERN_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("pattern_stitch_id")
            .ToTable(Names.Tables.PATTERN_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .Index()
            .OnTable(Names.Tables.USER_PATTERN_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("user_pattern_id");

        Create
            .Index("IX_user_pattern_stitch_user_pattern_id_x_y")
            .OnTable(Names.Tables.USER_PATTERN_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("user_pattern_id")
            .Ascending()
            .OnColumn("x")
            .Ascending()
            .OnColumn("y")
            .Ascending()
            .WithOptions()
            .Unique();
    }

    public override void Down()
    {
        Delete
            .Table(Names.Tables.USER_PATTERN_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}