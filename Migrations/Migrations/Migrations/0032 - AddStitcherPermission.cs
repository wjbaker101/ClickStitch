using FluentMigrator;
using Migrations.Common;
using Migrations.Records;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0032)]
public sealed class AddStitcherPermission_0032 : Migration
{
    public override void Up()
    {
        Insert
            .IntoTable(Names.Tables.PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .Row(PermissionRecord.Create(3, "Stitcher"));
    }

    public override void Down()
    {
        Delete
            .FromTable(Names.Tables.PERMISSION)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .Row(PermissionRecord.Create(3, "Stitcher"));
    }
}