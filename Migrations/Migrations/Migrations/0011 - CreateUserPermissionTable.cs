using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0011)]
public sealed class CreateUserPermissionTable_0011 : Migration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.USER_PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("permission_id").AsInt64().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable();

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("user_id")
            .ToTable(Names.Tables.USER)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("permission_id")
            .ToTable(Names.Tables.PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .Index()
            .OnTable(Names.Tables.USER_PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("user_id");

        Create
            .UniqueConstraint()
            .OnTable(Names.Tables.USER_PERMISSION)
            .WithSchema(Names.Schemas.CLICK_STITCH)
            .Columns("user_id", "permission_id");
    }

    public override void Down()
    {
        Delete
            .Table(Names.Tables.USER_PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}