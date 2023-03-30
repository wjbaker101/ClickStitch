using CrossStitchViewer.Models;

namespace CrossStitchViewer.Api.Users.Types;

public sealed class GetSelfResponse
{
    public required UserModel User { get; init; }
}