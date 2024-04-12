using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0054)]
public sealed class DropPatternThreadStitchTable_0054 : Migration
{
    public override void Up()
    {
        Delete
            .Table(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }

    public override void Down()
    {
    }
}