using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0020)]
public sealed class AddTitleSlugColumnToPatternTable_0020 : Migration
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

    public override void Down()
    {
        Delete
            .Column("title_slug")
            .FromTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}