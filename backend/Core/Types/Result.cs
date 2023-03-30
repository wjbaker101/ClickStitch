namespace Core.Types;

public class Result
{
    public bool IsFailure { get; }
    public bool IsSuccess => !IsFailure;

    private readonly string? _failureMessage;
    public string FailureMessage
    {
        get
        {
            if (IsSuccess)
                throw new Exception("Cannot access failure message, Result was a success;");

            return _failureMessage!;
        }
    }

    protected Result(bool isFailure, string? failureMessage)
    {
        IsFailure = isFailure;
        _failureMessage = failureMessage;
    }

    public static Result Success() => new(false, null);

    public static Result Failure(string failureMessage) => new(true, failureMessage);

    public static Result FromFailure<T>(Result<T> result) => Failure(result.FailureMessage);
}

public sealed class Result<TContent> : Result
{
    private readonly TContent? _content;
    public TContent Content
    {
        get
        {
            if (IsFailure)
                throw new Exception("Cannot access content, Result was a failure.");

            return _content!;
        }
    }

    private Result(TContent? content, bool isFailure, string? failureMessage) : base(isFailure, failureMessage)
    {
        _content = content;
    }

    public static Result<TContent> Of(TContent content) => new(content, false, null);

    public new static Result<TContent> Failure(string failureMessage) => new(default, true, failureMessage);

    public static implicit operator Result<TContent>(TContent value) => Of(value);

    public static Result<TContent> FromFailure(Result result) => Failure(result.FailureMessage);
    public new static Result<TContent> FromFailure<T>(Result<T> result) => Failure(result.FailureMessage);

    public bool TrySuccess(out TContent content)
    {
        if (IsFailure)
        {
            content = default!;
            return false;
        }

        content = Content;
        return true;
    }
}