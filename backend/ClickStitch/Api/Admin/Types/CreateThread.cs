namespace ClickStitch.Api.Admin.Types;

public sealed class CreateThreadRequest
{
    public required string Code { get; init; }
}

public sealed class CreateThreadResponse
{
    public required ThreadModel Thread { get; init; }
}