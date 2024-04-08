using FluentMigrator;
using Migrations.Common;
using Migrations.Extensions;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0050)]
public sealed class CreateUserPatternThreadTable_0050 : AutoReversingMigration
{
    public override void Up()
    {
        Alter
            .Table(Names.Tables.USER_PATTERN_THREAD_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .AddColumn("pattern_thread_id")
            .AsInt64()
            .Nullable()
            .AddColumn("x")
            .AsInt32()
            .Nullable()
            .AddColumn("y")
            .AsInt32()
            .Nullable()
            .AddColumn("completed_at")
            .AsTimestampWithTimeZone()
            .Nullable(); 
    }
}