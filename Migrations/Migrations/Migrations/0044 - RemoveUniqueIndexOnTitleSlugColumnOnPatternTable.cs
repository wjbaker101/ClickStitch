using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0044)]
public sealed class RemoveUniqueIndexOnTitleSlugColumnOnPatternTable_0044 : Migration
{
    public override void Up()
    {
        Delete
            .Index()
            .OnTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("title_slug");

        Create
            .Index()
            .OnTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("title_slug");
    }

    public override void Down()
    {
        Delete
            .Index()
            .OnTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("title_slug");

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