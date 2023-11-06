using FluentMigrator;
using Migrations.Common;
using Migrations.Extensions;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0026)]
public sealed class CreateThreadTable_0026 : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithIdColumn()
            .WithColumn("reference").AsGuid().NotNullable().Unique()
            .WithColumn("brand").AsString().NotNullable()
            .WithColumn("code").AsString().NotNullable();
    }
}