using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0003)]
public sealed class CreatePatternTable_0003 : Migration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("reference").AsGuid().NotNullable().Unique()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("title").AsString().NotNullable()
            .WithColumn("width").AsInt32().NotNullable()
            .WithColumn("height").AsInt32().NotNullable()
            .WithColumn("price").AsDecimal().NotNullable()
            .WithColumn("thumbnail_url").AsString().NotNullable()
            .WithColumn("thread_count").AsInt32().NotNullable()
            .WithColumn("stitch_count").AsInt32().NotNullable()
            .WithColumn("aida_count").AsInt32().NotNullable()
            .WithColumn("banner_image_url").AsString().Nullable()
            .WithColumn("external_shop_url").AsString().Nullable();
    }

    public override void Down()
    {
        Delete
            .Table(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}