using ClickStitch.Api.Admin;
using ClickStitch.Api.Auth;
using ClickStitch.Api.Creators;
using ClickStitch.Api.Inventory;
using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.Parsing;
using ClickStitch.Api.Patterns.Services;
using ClickStitch.Api.Projects;
using ClickStitch.Api.Users;
using ClickStitch.Clients.Cloudinary;
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

namespace ClickStitch.Setup;

public static class SetupDependencies
{
    public static void AddDependencies(this IServiceCollection services)
    {
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
        services.AddSingleton<IGetPatternInventoryService, GetPatternInventoryService>();

        services.AddSingleton<IProjectsService, ProjectsService>();

        services.AddSingleton<IUsersService, UsersService>();
    }
}