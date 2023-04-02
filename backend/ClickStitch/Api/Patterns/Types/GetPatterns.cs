using CrossStitchViewer.Models;

namespace CrossStitchViewer.Api.Patterns.Types;

public sealed class GetPatternsResponse
{
    public required List<PatternModel> Patterns { get; init; }
}