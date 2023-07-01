using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0022)]
public sealed class AddLookupHashColumnToPatternThreadStitchTable_0022 : Migration
{
    public override void Up()
    {
        Create
            .Column("lookup_hash")
            .OnTable(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AsString()
            .Nullable();

        Create
            .Index()
            .OnTable(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("pattern_thread_id")
            .Ascending()
            .OnColumn("lookup_hash")
            .Ascending()
            .WithOptions()
            .Unique();
    }

    public override void Down()
    {
        Delete
            .Index()
            .OnTable(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumns("pattern_thread_id", "lookup_hash");

        Delete
            .Column("lookup_hash")
            .FromTable(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}