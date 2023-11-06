using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0016)]
public sealed class AddCreatorToPatternTable_0016 : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Column("creator_id")
            .OnTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AsInt64()
            .Nullable();

        Create
            .ForeignKey()
            .FromTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("creator_id")
            .ToTable(Names.Tables.CREATOR)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");
    }
}