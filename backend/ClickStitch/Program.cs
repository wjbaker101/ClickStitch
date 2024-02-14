using ClickStitch.Setup;
using Core.Settings;
using Inkwell.Client;
using Inkwell.Client.Types;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.AddSettings();

var appSecrets = builder.Configuration.Get<AppSecrets>()!;
services.AddSingleton(appSecrets);

services.AddSingleton(builder.Configuration.Get<AppSettings>()!);

services.AddSingleton<IInkwellClient>(new InkwellClient(new InkwellClientOptions
{
    BaseUrl = appSecrets.Inkwell.BaseUrl,
    AppName = "ClickStitch"
}));

services.AddMiddleware();
services.AddDependencies();
services.AddControllers();
services.AddFrontend();

var app = builder.Build();

app.UseMiddleware();
app.UseAuthorization();
app.MapControllers();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseFrontend();

app.Run();

public partial class Program; // For integration tests