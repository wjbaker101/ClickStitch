using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0014)]
public sealed class CreateUserCreatorTable_0014 : Migration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.USER_CREATOR)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt64().NotNullable().Unique()
            .WithColumn("creator_id").AsInt64().NotNullable();

        Create
            .Index()
            .OnTable(Names.Tables.USER_CREATOR)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("creator_id")
            .Ascending();
    }

    public override void Down()
    {
        Delete
            .Table(Names.Tables.USER_CREATOR)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}