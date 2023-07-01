using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0023)]
public sealed class SetLookupHashColumnAsNonNullable_0023 : Migration
{
    public override void Up()
    {
        Alter
            .Column("lookup_hash")
            .OnTable(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AsString()
            .NotNullable();
    }

    public override void Down()
    {
        Alter
            .Column("lookup_hash")
            .OnTable(Names.Tables.PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AsString()
            .Nullable();
    }
}