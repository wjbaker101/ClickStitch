using CrossStitchViewer.Models;
using Data.Records;

namespace CrossStitchViewer.Models.Mappers;

public static class BasketMapper
{
    public static BasketModel Map(List<UserBasketItemRecord> items, decimal totalPrice) => new()
    {
        Items = items.ConvertAll(x => MapItem(x.Pattern, x.CreatedAt)),
        TotalPrice = totalPrice
    };

    public static BasketItemModel MapItem(PatternRecord pattern, DateTime addedAt) => new()
    {
        Pattern = PatternMapper.Map(pattern),
        AddedAt = addedAt
    };
}