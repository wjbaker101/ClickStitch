namespace Core.Settings;

public sealed class AppSecrets
{
    public required DatabaseSettings Database { get; init; }
    public required CloudinarySettings Cloudinary { get; init; }
    public required AuthSettings Auth { get; init; }
    public required InkwellSettings Inkwell { get; init; }

    public sealed class DatabaseSettings
    {
        public required string Host { get; init; }
        public required int Port { get; init; }
        public required string Database { get; init; }
        public required string Username { get; init; }
        public required string Password { get; init; }
    }

    public sealed class CloudinarySettings
    {
        public required string CloudName { get; init; }
        public required string ApiKey { get; init; }
        public required string ApiSecret { get; init; }
    }

    public sealed class AuthSettings
    {
        public required LoginTokenSettings LoginToken { get; init; }
        public required PasswordSettings Password { get; init; }

        public sealed class LoginTokenSettings
        {
            public required string SecretKey { get; init; }
        }

        public sealed class PasswordSettings
        {
            public required string Pepper { get; init; }
        }
    }

    public sealed class InkwellSettings
    {
        public required string BaseUrl { get; init; }
    }
}