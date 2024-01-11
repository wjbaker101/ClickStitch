namespace Data.Repositories.UserThread.Types;

public sealed class SearchUserThreadsParameters
{
    public required string? SearchTerm { get; init; }
    public required string? Brand { get; init; }
}