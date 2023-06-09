﻿namespace ClickStitch.Api.Creators.Types;

public sealed class GetCreatorPatternsResponse
{
    public required List<PatternModel> Patterns { get; init; }
    public required PaginationModel Pagination { get; init; }
}