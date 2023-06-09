﻿namespace ClickStitch.Api.Creators.Types;

public sealed class CreateCreatorRequest
{
    public required string Name { get; init; }
    public required string StoreUrl { get; init; }
}

public sealed class CreateCreatorResponse
{
    public required CreatorModel Creator { get; init; }
}