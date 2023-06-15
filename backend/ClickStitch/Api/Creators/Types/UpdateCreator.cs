namespace ClickStitch.Api.Creators.Types;

public sealed class UpdateCreatorRequest
{
    public required string Name { get; init; }
    public required string StoreUrl { get; init; }
}

public sealed class UpdateCreatorResponse
{
    public required CreatorModel Creator { get; init; }
}