namespace Data.Repositories.Thread.Types;

public sealed class SearchThreadsParameters
{
    public required string? SearchTerm { get; init; }
    public required string? Brand { get; init; }
}