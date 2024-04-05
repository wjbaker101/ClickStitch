namespace ClickStitch.Api.Creators.Types;

public sealed class GetCreatorByUserResponse
{
    public required CreatorModel? Creator { get; init; }
}