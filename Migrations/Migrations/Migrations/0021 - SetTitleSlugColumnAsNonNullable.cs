using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0021)]
public sealed class SetTitleSlugColumnAsNonNullable_0021 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Column("title_slug")
            .OnTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AsString()
            .NotNullable();
    }
}