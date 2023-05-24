using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0013)]
public sealed class CreateCreatorTable_0013 : Migration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.CREATOR)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("reference").AsGuid().NotNullable().Unique()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("name").AsString().NotNullable().Unique()
            .WithColumn("store_url").AsString().NotNullable();
    }

    public override void Down()
    {
        Delete
            .Table(Names.Tables.CREATOR)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}