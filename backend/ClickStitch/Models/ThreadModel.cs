﻿namespace ClickStitch.Models;

public sealed class ThreadModel
{
    public required Guid Reference { get; init; }
    public required string Brand { get; init; }
    public required string Code { get; init; }
    public required string Colour { get; init; }
}