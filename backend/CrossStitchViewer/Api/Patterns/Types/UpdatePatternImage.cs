using Microsoft.AspNetCore.Mvc;

namespace CrossStitchViewer.Api.Patterns.Types;

public sealed class UpdatePatternImageRequest
{
    [FromForm]
    public required IFormFile File { get; init; }

    [FromForm]
    public required string FileName { get; init; }
}