namespace ClickStitch.Setup;

public static class SetupSettings
{
    public static void AddSettings(this WebApplicationBuilder builder)
    {
        var isDev = builder.Environment.IsDevelopment();

        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile(GetFile("appsettings", isDev))
            .AddJsonFile(GetFile("appsecrets", isDev));
    }

    private static string GetFile(string file, bool isDev)
    {
        return isDev ? $"{file}.Development.json" : $"{file}.json";
    }
}