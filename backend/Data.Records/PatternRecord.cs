using Data.Records.Types;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class PatternRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required string Title { get; set; }
    public virtual required int Width { get; set; }
    public virtual required int Height { get; set; }
    public virtual required decimal Price { get; set; }
}

public sealed class PatternRecordMap : ClassMap<PatternRecord>
{
    public PatternRecordMap()
    {
        Schema("cross_stitch_viewer");
        Table("pattern");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("pattern_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Title, "title");
        Map(x => x.Width, "width");
        Map(x => x.Height, "height");
        Map(x => x.Price, "price");
    }
}