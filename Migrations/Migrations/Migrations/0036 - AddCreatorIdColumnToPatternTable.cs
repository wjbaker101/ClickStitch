using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0036)]
public sealed class AddCreatorIdColumnToPatternTable_0036 : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Column("creator_id")
            .OnTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AsInt64()
            .Nullable();
    }
}