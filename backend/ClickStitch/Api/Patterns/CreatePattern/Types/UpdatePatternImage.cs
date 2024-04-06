using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Patterns.CreatePattern.Types;

public sealed class UpdatePatternImageRequest
{
    [FromForm]
    public required IFormFile File { get; init; }

    [FromForm]
    public required string FileName { get; init; }
}