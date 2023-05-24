using FluentMigrator;
using Migrations.Common;
using Migrations.Records;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0015)]
public sealed class AddCreatorPermission_0015 : Migration
{
    public override void Up()
    {
        Insert
            .IntoTable(Names.Tables.PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .Row(PermissionRecord.Create(2, "Creator"));
    }

    public override void Down()
    {
        Delete
            .FromTable(Names.Tables.PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .Row(PermissionRecord.Create(2, "Creator"));
    }
}