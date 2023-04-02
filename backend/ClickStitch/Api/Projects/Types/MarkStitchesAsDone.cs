namespace ClickStitch.Api.Projects.Types;

public sealed class MarkStitchesAsDoneRequest
{
    public required List<Guid> References { get; init; }
}