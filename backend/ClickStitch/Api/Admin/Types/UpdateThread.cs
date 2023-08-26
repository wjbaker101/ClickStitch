namespace ClickStitch.Api.Admin.Types;

public sealed class UpdateThreadRequest
{
    public required string Brand { get; init; }
    public required string Code { get; init; }
}

public sealed class UpdateThreadResponse
{
    public required ThreadModel Thread { get; init; }
}