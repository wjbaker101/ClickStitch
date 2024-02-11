using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0047)]
public sealed class MakeReferenceColumnOnUserPatternTableNonNullable_0047 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.USER_PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AlterColumn("reference")
            .AsGuid()
            .NotNullable();
    }
}