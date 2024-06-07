using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0058)]
public sealed class AddDescriptionColumnToCreatorTable_0058 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.CREATOR)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AddColumn("description")
            .AsString()
            .Nullable();
    }
}