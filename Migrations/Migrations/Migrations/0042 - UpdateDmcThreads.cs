using FluentMigrator;
using Migrations.Common;
using Migrations.Records;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0042)]
public sealed class UpdateDmcThreads_0042 : Migration
{
    public override void Up()
    {
        Insert
            .IntoTable(Names.Tables.THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .Row(ThreadRecord.Create(Guid.NewGuid(), "DMC", "09", "#4b3635"));
    }

    public override void Down()
    {
    }
}