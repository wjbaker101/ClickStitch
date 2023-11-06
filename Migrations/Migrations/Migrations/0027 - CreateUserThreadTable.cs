using FluentMigrator;
using Migrations.Common;
using Migrations.Extensions;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0027)]
public sealed class CreateUserThreadTable_0027 : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.USER_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithIdColumn()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("thread_id").AsInt64().NotNullable()
            .WithColumn("count").AsInt32().NotNullable();

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("user_id")
            .ToTable(Names.Tables.USER)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .ForeignKey()
            .FromTable(Names.Tables.USER_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("thread_id")
            .ToTable(Names.Tables.THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .Index()
            .OnTable(Names.Tables.USER_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("user_id");

        Create
            .Index()
            .OnTable(Names.Tables.USER_THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("thread_id");

        Create
            .UniqueConstraint()
            .OnTable(Names.Tables.USER_THREAD)
            .WithSchema(Names.Schemas.CLICK_STITCH)
            .Columns("user_id", "thread_id");
    }
}