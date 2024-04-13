using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0057)]
public sealed class AddIndexAndForeignKeyToUserPatternThreadStitchTable_0057 : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("pattern_thread_id")
            .ToTable(Names.Tables.PATTERN_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .Index()
            .OnTable(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("user_id")
            .Ascending()
            .OnColumn("pattern_thread_id")
            .Ascending();
    }
}