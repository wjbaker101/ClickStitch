using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0046)]
public sealed class AddReferenceColumnToUserPatternTable_0046 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.USER_PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AddColumn("reference")
            .AsGuid()
            .Nullable();

        Create
            .Index()
            .OnTable(Names.Tables.USER_PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("reference")
            .Unique();
    }
}