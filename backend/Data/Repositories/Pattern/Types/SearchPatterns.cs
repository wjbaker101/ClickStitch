using Data.Records;

namespace Data.Repositories.Pattern.Types;

public sealed class SearchPatternsParameters
{
    public required List<PatternRecord> PatternsToExclude { get; init; }
}