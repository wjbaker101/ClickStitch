namespace Core.Settings;

public sealed class AppSettings
{
    public required string Environment { get; init; }
}

public sealed class Environments
{
    public const string PRODUCTION = "PRODUCTION";
    public const string LOCAL = "LOCAL";
}