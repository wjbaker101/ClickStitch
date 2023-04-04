using ClickStitch.Api.Auth;
using ClickStitch.Api.Basket;
using ClickStitch.Api.Patterns;
using ClickStitch.Api.Projects;
using ClickStitch.Api.Users;
using ClickStitch.Clients.Cloudinary;
using ClickStitch.Filters;
using Core.Services;
using Core.Settings;
using Data;
using Data.Repositories.Basket;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternStitch;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

SetupSettings();
services.AddSingleton(builder.Configuration.Get<AppSecrets>()!);

services.AddSingleton<IDateTime, DateTimeProvider>();
services.AddSingleton<IGuid, GuidProvider>();

services.AddSingleton<IDatabase, Database>();
services.AddSingleton<IBasketRepository, BasketRepository>();
services.AddSingleton<IPatternRepository, PatternRepository>();
services.AddSingleton<IPatternStitchRepository, PatternStitchRepository>();
services.AddSingleton<IPatternThreadRepository, PatternThreadRepository>();
services.AddSingleton<IUserRepository, UserRepository>();
services.AddSingleton<IUserPatternRepository, UserPatternRepository>();
services.AddSingleton<IUserPatternStitchRepository, UserPatternStitchRepository>();

services.AddSingleton<ICloudinaryClient, CloudinaryClient>();

services.AddSingleton<IBasketService, BasketService>();

services.AddSingleton<IAuthService, AuthService>();
services.AddSingleton<ILoginTokenService, LoginTokenService>();
services.AddSingleton<IPasswordService, PasswordService>();

services.AddSingleton<IPatternsService, PatternsService>();

services.AddSingleton<IProjectsService, ProjectsService>();

services.AddSingleton<IUsersService, UsersService>();

services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});

services.AddSpaStaticFiles(spa =>
{
    spa.RootPath = "wwwroot";
});

var app = builder.Build();

app.UseHttpsRedirection();
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