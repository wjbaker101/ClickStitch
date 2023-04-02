namespace ClickStitch.Types;

public abstract class ApiResponse
{
    public DateTime ResponseAt => DateTime.UtcNow;
}

public sealed class ApiResultResponse<TResult> : ApiResponse
{
    public required TResult Result { get; init; }
}

public sealed class ApiErrorResponse : ApiResponse
{
    public required string FailureMessage { get; init; }
}