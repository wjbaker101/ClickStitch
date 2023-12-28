using ClickStitch.Api.Admin;
using ClickStitch.Api.Auth;
using ClickStitch.Api.Creators;
using ClickStitch.Api.Inventory;
using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.Parsing;
using ClickStitch.Api.Projects;
using ClickStitch.Api.Users;
using ClickStitch.Clients.Cloudinary;
using ClickStitch.Filters;
using Core.Settings;
using Data;
using Data.Repositories.Admin;
using Data.Repositories.Creator;
using Data.Repositories.Pattern;
using Data.Repositories.Permission;
using Data.Repositories.Thread;
using Data.Repositories.User;
using Data.Repositories.UserCreator;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternThreadStitch;
using Data.Repositories.UserPermission;
using Data.Repositories.UserThread;
using DotNetLibs.Core.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

SetupSettings();
services.AddSingleton(builder.Configuration.Get<AppSecrets>()!);

services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
services.AddSingleton<IGuidProvider, GuidProvider>();

services.AddSingleton<IDatabase, Database>();
services.AddSingleton<IAdminRepository, AdminRepository>();
services.AddSingleton<ICreatorRepository, CreatorRepository>();
services.AddSingleton<IPatternRepository, PatternRepository>();
services.AddSingleton<IPatternThreadRepository, PatternThreadRepository>();
services.AddSingleton<IPatternThreadStitchRepository, PatternThreadStitchRepository>();
services.AddSingleton<IUserRepository, UserRepository>();
services.AddSingleton<IUserPatternRepository, UserPatternRepository>();
services.AddSingleton<IPermissionRepository, PermissionRepository>();
services.AddSingleton<IThreadRepository, ThreadRepository>();
services.AddSingleton<IUserCreatorRepository, UserCreatorRepository>();
services.AddSingleton<IUserPermissionRepository, UserPermissionRepository>();
services.AddSingleton<IUserPatternThreadStitchRepository, UserPatternThreadStitchRepository>();
services.AddSingleton<IUserThreadRepository, UserThreadRepository>();

services.AddSingleton<ICloudinaryClient, CloudinaryClient>();

services.AddSingleton<IAdminService, AdminService>();

services.AddSingleton<ICreatorsService, CreatorsService>();

services.AddSingleton<IInventoryService, InventoryService>();

services.AddSingleton<IAuthService, AuthService>();
services.AddSingleton<ILoginTokenService, LoginTokenService>();
services.AddSingleton<IPasswordService, PasswordService>();

services.AddSingleton<IPatternsService, PatternsService>();
services.AddSingleton<IPatternUploadService, PatternUploadService>();
services.AddSingleton<IPatternParserService, PatternParserService>();

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

//app.UseHttpsRedirection();
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