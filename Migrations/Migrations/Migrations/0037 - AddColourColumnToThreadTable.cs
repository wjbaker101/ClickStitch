using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0037)]
public sealed class AddColourColumnToThreadTable_0037 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AddColumn("colour")
            .AsString()
            .Nullable();
    }
}