using ClickStitch.Setup;
using Core.Settings;
using Inkwell.Client;
using Inkwell.Client.Types;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

SetupSettings();
var appSecrets = builder.Configuration.Get<AppSecrets>()!;

services.AddSingleton(appSecrets);

services.AddSingleton<IInkwellClient>(new InkwellClient(new InkwellClientOptions
{
    BaseUrl = appSecrets.Inkwell.BaseUrl,
    AppName = "ClickStitch"
}));

services.AddMiddleware();
services.AddDependencies();
services.AddControllers();

services.AddSpaStaticFiles(spa =>
{
    spa.RootPath = "wwwroot";
});

var app = builder.Build();

app.UseMiddleware();
app.UseAuthorization();
app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "wwwroot";
});

app.Run();

void SetupSettings()
{
    var isDev = builder.Environment.IsDevelopment();

    builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile(GetFile("appsettings", isDev))
        .AddJsonFile(GetFile("appsecrets", isDev));
}

string GetFile(string file, bool isDev) => isDev ? $"{file}.Development.json" : $"{file}.json";

public partial class Program; // For integration tests