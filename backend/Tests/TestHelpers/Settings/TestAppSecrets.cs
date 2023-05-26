using Core.Settings;

namespace TestHelpers.Settings;

public sealed class TestAppSecrets
{
    public static implicit operator AppSecrets(TestAppSecrets _) => new()
    {
        Database = new AppSecrets.DatabaseSettings
        {
            Host = "TestHost",
            Port = 9790,
            Database = "TestDatabase",
            Username = "TestUsername",
            Password = "TestPassword"
        },
        Cloudinary = new AppSecrets.CloudinarySettings
        {
            CloudName = "TestCloudName",
            ApiKey = "TestApiKey",
            ApiSecret = "TestApiSecret"
        },
        Auth = new AppSecrets.AuthSettings
        {
            LoginToken = new AppSecrets.AuthSettings.LoginTokenSettings
            {
                SecretKey = "TestSecret_e38a3038-44b5-4955-a4df-4c41b3f13518"
            },
            Password = new AppSecrets.AuthSettings.PasswordSettings
            {
                Pepper = "TestPepper"
            }
        }
    };
}