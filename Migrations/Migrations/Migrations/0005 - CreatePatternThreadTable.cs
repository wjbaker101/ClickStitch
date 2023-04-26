using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0005)]
public sealed class CreatePatternThreadTable_0005 : Migration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.PATTERN_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("pattern_id").AsInt64().NotNullable()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("description").AsString().NotNullable()
            .WithColumn("index").AsInt32().NotNullable()
            .WithColumn("colour").AsString().Nullable();

        Create
            .ForeignKey()
            .FromTable(Names.Tables.PATTERN_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("pattern_id")
            .ToTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .Index()
            .OnTable(Names.Tables.PATTERN_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("pattern_id");
    }

    public override void Down()
    {
        Delete
            .Table(Names.Tables.PATTERN_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}