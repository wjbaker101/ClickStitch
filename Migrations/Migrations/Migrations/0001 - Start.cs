﻿using FluentMigrator;
using Migrations.Common;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0001)]
public sealed class Start_0001 : AutoReversingMigration
{
    public override void Up()
    {
        Create.Schema(Names.Schemas.CLICK_STITCH);
    }
}