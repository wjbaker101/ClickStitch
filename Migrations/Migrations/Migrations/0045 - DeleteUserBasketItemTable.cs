using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0045)]
public sealed class DeleteUserBasketItemTable_0045 : Migration
{
    public override void Up()
    {
        Delete
            .Table(Names.Tables.USER_BASKET_ITEM)
            .InSchema(Names.Schemas.CLICK_STITCH);
    }

    public override void Down()
    {
        Create
            .Table(Names.Tables.USER_BASKET_ITEM)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("pattern_id").AsInt64().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable();

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_BASKET_ITEM)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("user_id")
            .ToTable(Names.Tables.USER)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_BASKET_ITEM)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("pattern_id")
            .ToTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .Index()
            .OnTable(Names.Tables.USER_BASKET_ITEM)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("user_id");

        Create
            .Index("IX_user_basket_item_user_id_pattern_id")
            .OnTable(Names.Tables.USER_BASKET_ITEM)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("user_id, pattern_id")
            .Unique();
    }
}