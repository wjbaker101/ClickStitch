using Core.Settings;

namespace TestHelpers.Settings;

public sealed class TestAppSecrets
{
    public TestDatabaseSettings Database { get; } = new();
    public TestCloudinarySettings Cloudinary { get; } = new();
    public TestAuthSettings Auth { get; } = new();
    public TestInkwellSettings Inkwell { get; } = new();

    public sealed class TestDatabaseSettings
    {
        public string Host { get; set; } = "TestHost";
        public int Port { get; set; } = 9790;
        public string Database { get; set; } = "TestDatabase";
        public string Username { get; set; } = "TestUsername";
        public string Password { get; set; } = "TestPassword";
    }

    public sealed class TestCloudinarySettings
    {
        public string CloudName { get; set; } = "TestCloudName";
        public string ApiKey { get; set; } = "TestApiKey";
        public string ApiSecret { get; set; } = "TestApiSecret";
    }

    public sealed class TestAuthSettings
    {
        public TestLoginTokenSettings LoginToken { get; } = new(); 
        public TestPasswordSettings Password { get; } = new();

        public sealed class TestLoginTokenSettings
        {
            public string SecretKey { get; set; } = "TestSecret_e38a3038-44b5-4955-a4df-4c41b3f13518_e556cc10-0771-4324-aff5-f94d9ed6a93a";
        }

        public sealed class TestPasswordSettings
        {
            public string Pepper { get; set; } = "TestPepper";
        }
    }

    public sealed class TestInkwellSettings
    {
        public string BaseUrl { get; set; } = "TestInkwellUrl";
    }

    public static implicit operator AppSecrets(TestAppSecrets secrets) => new()
    {
        Database = new AppSecrets.DatabaseSettings
        {
            Host = secrets.Database.Host,
            Port = secrets.Database.Port,
            Database = secrets.Database.Database,
            Username = secrets.Database.Username,
            Password = secrets.Database.Password
        },
        Cloudinary = new AppSecrets.CloudinarySettings
        {
            CloudName = secrets.Cloudinary.CloudName,
            ApiKey = secrets.Cloudinary.ApiKey,
            ApiSecret = secrets.Cloudinary.ApiSecret
        },
        Auth = new AppSecrets.AuthSettings
        {
            LoginToken = new AppSecrets.AuthSettings.LoginTokenSettings
            {
                SecretKey = secrets.Auth.LoginToken.SecretKey
            },
            Password = new AppSecrets.AuthSettings.PasswordSettings
            {
                Pepper = secrets.Auth.Password.Pepper
            }
        },
        Inkwell = new AppSecrets.InkwellSettings
        {
            BaseUrl = secrets.Inkwell.BaseUrl
        }
    };
}