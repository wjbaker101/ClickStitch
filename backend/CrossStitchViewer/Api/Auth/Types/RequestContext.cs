using CrossStitchViewer.Models;

namespace CrossStitchViewer.Api.Auth.Types;

public sealed class RequestContext
{
    public required UserModel User { get; init; }
}