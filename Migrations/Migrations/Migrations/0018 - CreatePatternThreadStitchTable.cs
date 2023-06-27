using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0018)]
public sealed class CreatePatternThreadStitchTable_0018 : Migration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("pattern_thread_id").AsInt64().NotNullable()
            .WithColumn("x").AsInt32().NotNullable()
            .WithColumn("y").AsInt32().NotNullable();

        Create
            .ForeignKey()
            .FromTable(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("pattern_thread_id")
            .ToTable(Names.Tables.PATTERN_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .Index()
            .OnTable(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("pattern_thread_id").Ascending()
            .OnColumn("x").Ascending()
            .OnColumn("y").Ascending()
            .WithOptions()
            .Unique();
    }

    public override void Down()
    {
        Delete
            .Table(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}