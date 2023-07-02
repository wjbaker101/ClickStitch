namespace Migrations;

public sealed class ConnectionStringParameters
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Database { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public static class ConnectionStringBuilder
{
    public static string Build(ConnectionStringParameters parameters)
    {
        var properties = new Dictionary<string, string>
        {
            ["Server"] = parameters.Host,
            ["Port"] = parameters.Port.ToString(),
            ["Database"] = parameters.Database,
            ["User Id"] = parameters.Username,
            ["Password"] = parameters.Password
        };

        return string.Join(";", properties.Select(x => $"{x.Key}={x.Value}"));
    }
}