using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0041)]
public sealed class SetColourColumnInThreadTableToNotNull_0041 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AlterColumn("colour")
            .AsString()
            .NotNullable();
    }
}