using Data.Records.Types;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class UserBasketItemRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required UserRecord User { get; init; }
    public virtual required PatternRecord Pattern { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
}

public sealed class UserBasketItemRecordMap : ClassMap<UserBasketItemRecord>
{
    public UserBasketItemRecordMap()
    {
        Schema("cross_stitch_viewer");
        Table("user_basket_item");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_basket_item_id_seq");
        References(x => x.User, "user_id");
        References(x => x.Pattern, "pattern_id");
        Map(x => x.CreatedAt, "created_at");
    }
}