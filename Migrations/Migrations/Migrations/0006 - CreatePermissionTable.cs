using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0006)]
public sealed class CreatePermissionTable_0006 : Migration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("type").AsInt32().NotNullable()
            .WithColumn("name").AsString().NotNullable();
    }

    public override void Down()
    {
        Delete
            .Table(Names.Tables.PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}