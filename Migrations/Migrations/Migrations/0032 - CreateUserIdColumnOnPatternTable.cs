using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0032)]
public sealed class CreateUserIdColumnOnPatternTable_0032 : Migration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AddColumn("user_id")
            .AsInt64()  
            .Nullable();

        Create
            .ForeignKey()
            .FromTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("user_id")
            .ToTable(Names.Tables.USER)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete
            .Column("user_id")
            .FromTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }
}