using FluentMigrator;
using Migrations.Common;
using Migrations.Records;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0007)]
public sealed class AddAdminPermission_0007 : Migration
{
    public override void Up()
    {
        Insert
            .IntoTable(Names.Tables.PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .Row(PermissionRecord.Create(1, "Admin"));
    }

    public override void Down()
    {
        Delete
            .FromTable(Names.Tables.PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .Row(PermissionRecord.Create(1, "Admin"));
    }
}