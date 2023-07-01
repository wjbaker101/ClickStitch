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
    public virtual required string? ThumbnailUrl { get; set; }
    public virtual required int ThreadCount { get; set; }
    public virtual required int StitchCount { get; set; }
    public virtual required int AidaCount { get; set; }
    public virtual required string BannerImageUrl { get; set; }
    public virtual required string? ExternalShopUrl { get; set; }
    public virtual required CreatorRecord Creator { get; init; }
    public virtual required string? TitleSlug { get; init; }
    public virtual required ISet<PatternStitchRecord> Stitches { get; init; }
    public virtual required ISet<PatternThreadRecord> Threads { get; init; }
}

public sealed class PatternRecordMap : ClassMap<PatternRecord>
{
    public PatternRecordMap()
    {
        Schema(DatabaseValues.SCHEMA);
        Table("pattern");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("pattern_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Title, "title");
        Map(x => x.Width, "width");
        Map(x => x.Height, "height");
        Map(x => x.Price, "price");
        Map(x => x.ThumbnailUrl, "thumbnail_url");
        Map(x => x.ThreadCount, "thread_count");
        Map(x => x.StitchCount, "stitch_count");
        Map(x => x.AidaCount, "aida_count");
        Map(x => x.BannerImageUrl, "banner_image_url");
        Map(x => x.ExternalShopUrl, "external_shop_url");
        Map(x => x.TitleSlug, "title_slug");
        References(x => x.Creator, "creator_id");
        HasMany(x => x.Stitches).KeyColumn("pattern_id");
        HasMany(x => x.Threads).KeyColumn("pattern_id");
    }
}