using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0002)]
public sealed class CreateUserTable_0002 : Migration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.USER)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("reference").AsGuid().NotNullable().Unique()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("email").AsString().NotNullable()
            .WithColumn("password").AsString().NotNullable()
            .WithColumn("password_salt").AsString().NotNullable();

        Create
            .Index("IX_user_lower_email")
            .OnTable(Names.Tables.USER)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("lower(email)")
            .Unique();
    }

    public override void Down()
    {
        Delete.Index("IX_user_lower_email");

        Delete
            .Table(Names.Tables.USER)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}