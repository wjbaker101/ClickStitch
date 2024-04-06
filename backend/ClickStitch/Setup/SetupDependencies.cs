using ClickStitch.Api.Admin.AssignPermissionToUser;
using ClickStitch.Api.Admin.GetPermissions;
using ClickStitch.Api.Admin.RemovePermissionFromUser;
using ClickStitch.Api.Admin.SearchUsers;
using ClickStitch.Api.Auth;
using ClickStitch.Api.Auth.LogIn;
using ClickStitch.Api.Creators.CreateCreator;
using ClickStitch.Api.Creators.GetCreatorBySelf;
using ClickStitch.Api.Creators.GetCreatorPatterns;
using ClickStitch.Api.Creators.UpdateCreator;
using ClickStitch.Api.Inventory.SearchInventoryThreads;
using ClickStitch.Api.Inventory.UpdateInventoryThread;
using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.Parsing;
using ClickStitch.Api.Patterns.Services;
using ClickStitch.Api.Projects.AddProject;
using ClickStitch.Api.Projects.CompleteStitches;
using ClickStitch.Api.Projects.GetAnalytics;
using ClickStitch.Api.Projects.GetProject;
using ClickStitch.Api.Projects.GetProjects;
using ClickStitch.Api.Projects.PauseStitching;
using ClickStitch.Api.Projects.UnCompleteStitches;
using ClickStitch.Api.Projects.UnPauseStitching;
using ClickStitch.Api.Threads.GetThreadsByColour;
using ClickStitch.Api.Users.CreateUser;
using ClickStitch.Api.Users.DeleteUser;
using ClickStitch.Api.Users.GetUserBySelf;
using ClickStitch.Api.Users.UpdateUser;
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

        services.AddSingleton<IAssignPermissionToUserService, AssignPermissionToUserService>();
        services.AddSingleton<IGetPermissionsService, GetPermissionsService>();
        services.AddSingleton<IRemovePermissionFromUserService, RemovePermissionFromUserService>();
        services.AddSingleton<ISearchUsersService, SearchUsersService>();

        services.AddSingleton<ICreateCreatorService, CreateCreatorService>();
        services.AddSingleton<IGetCreatorBySelfService, GetCreatorBySelfService>();
        services.AddSingleton<IGetCreatorPatternsService, GetCreatorPatternsService>();
        services.AddSingleton<IUpdateCreatorService, UpdateCreatorService>();

        services.AddSingleton<ISearchInventoryThreadsService, SearchInventoryThreadsService>();
        services.AddSingleton<IUpdateInventoryThreadService, UpdateInventoryThreadService>();

        services.AddSingleton<ILogInService, LogInService>();
        services.AddSingleton<ILoginTokenService, LoginTokenService>();
        services.AddSingleton<IPasswordService, PasswordService>();

        services.AddSingleton<IPatternsService, PatternsService>();
        services.AddSingleton<IPatternUploadService, PatternUploadService>();
        services.AddSingleton<IPatternParserService, PatternParserService>();
        services.AddSingleton<IGetPatternInventoryService, GetPatternInventoryService>();
        services.AddSingleton<IGetPatternService, GetPatternService>();

        services.AddSingleton<IAddProjectService, AddProjectService>();
        services.AddSingleton<ICompleteStitchesService, CompleteStitchesService>();
        services.AddSingleton<IGetAnalyticsService, GetAnalyticsService>();
        services.AddSingleton<IGetProjectService, GetProjectService>();
        services.AddSingleton<IGetProjectsService, GetProjectsService>();
        services.AddSingleton<IPauseStitchingService, PauseStitchingService>();
        services.AddSingleton<IUnCompleteStitchesService, UnCompleteStitchesService>();
        services.AddSingleton<IUnPauseStitchingService, UnPauseStitchingService>();

        services.AddSingleton<IGetThreadsByColourService, GetThreadsByColourService>();

        services.AddSingleton<ICreateUserService, CreateUserService>();
        services.AddSingleton<IDeleteUserService, DeleteUserService>();
        services.AddSingleton<IGetUserBySelfService, GetUserByUserBySelfService>();
        services.AddSingleton<IUpdateUserService, UpdateUserService>();
    }
}