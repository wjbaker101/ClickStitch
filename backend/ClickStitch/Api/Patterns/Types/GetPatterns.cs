using ClickStitch.Models;

namespace ClickStitch.Api.Patterns.Types;

public sealed class GetPatternsResponse
{
    public required List<PatternModel> Patterns { get; init; }
}