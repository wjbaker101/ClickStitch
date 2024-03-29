﻿using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0004)]
public sealed class CreatePatternStitchTable_0004 : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Table(Names.Tables.PATTERN_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("pattern_id").AsInt64().NotNullable()
            .WithColumn("thread_index").AsInt32().NotNullable()
            .WithColumn("x").AsInt32().NotNullable()
            .WithColumn("y").AsInt32().NotNullable();

        Create
            .ForeignKey()
            .FromTable(Names.Tables.PATTERN_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .ForeignColumn("pattern_id")
            .ToTable(Names.Tables.PATTERN)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .PrimaryColumn("id");

        Create
            .Index()
            .OnTable(Names.Tables.PATTERN_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("pattern_id");

        Create
            .Index("IX_pattern_stitch_pattern_id_x_y")
            .OnTable(Names.Tables.PATTERN_STITCH)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .OnColumn("pattern_id, x, y")
            .Unique();
    }
}