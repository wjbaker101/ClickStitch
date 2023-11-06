using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0020)]
public sealed class AddTitleSlugColumnToPatternTable_0020 : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Column("title_slug")
            .OnTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AsString()
            .Nullable();

        Create
            .Index()
            .OnTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("title_slug")
            .Ascending()
            .WithOptions()
            .Unique();
    }
}