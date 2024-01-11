using FluentMigrator;
using Migrations.Common;
using Migrations.Records;

namespace Migrations.Migrations;

// ReSharper disable once InconsistentNaming
[Migration(0043)]
public sealed class UpdateAnchorThreads_0043 : Migration
{
    public override void Up()
    {
        Insert
            .IntoTable(Names.Tables.THREAD)
            .InSchema(Names.Schemas.CLICK_STITCH)
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01345", "#bddeb3"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01355", "#4a5e56"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01360", "#c25270"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01318", "#dead91"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01344", "#d3d7dc"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01347", "#568bb1"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01352", "#a2b091"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01305", "#e3953f"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01315", "#e5855e"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "00851", "#2f5160"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01301", "#ddd2bf"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01300", "#caba9a"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01385", "#d8874b"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01342", "#d0e2de"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01316", "#c12e30"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01390", "#734f40"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "00242", "#7aa972"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01349", "#47608f"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01302", "#e8ded0"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01320", "#eec7ce"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01303", "#eecc7c"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01335", "#cf919a"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01304", "#ebce74"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01325", "#b775a3"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01353", "#bebd83"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "01375", "#b5833a"))
            .Row(ThreadRecord.Create(Guid.NewGuid(), "Anchor", "00271", "#f3d9cd"));
    }

    public override void Down()
    {
    }
}