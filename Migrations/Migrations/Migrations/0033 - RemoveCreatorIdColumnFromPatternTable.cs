using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0033)]
public sealed class RemoveCreatorIdColumnFromPatternTable_0033 : Migration
{
    public override void Up()
    {
        Delete
            .Column("creator_id")
            .FromTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }

    public override void Down()
    {
    }
}