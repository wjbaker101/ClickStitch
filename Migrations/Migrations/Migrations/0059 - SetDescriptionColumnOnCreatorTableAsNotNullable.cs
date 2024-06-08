using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0059)]
public sealed class SetDescriptionColumnOnCreatorTableAsNotNullable_0059 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.CREATOR)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AlterColumn("description")
            .AsString()
            .NotNullable();
    }
}