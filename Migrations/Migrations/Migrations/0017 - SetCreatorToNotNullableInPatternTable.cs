using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0017)]
public sealed class SetCreatorToNotNullableInPatternTable_0017 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AlterColumn("creator_id")
            .AsInt64()
            .NotNullable();
    }
}