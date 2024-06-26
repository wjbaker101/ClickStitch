﻿using ClickStitch.Api.Admin.AssignPermissionToUser;
using ClickStitch.Api.Admin.GetPermissions;
using ClickStitch.Api.Admin.RemovePermissionFromUser;
using ClickStitch.Api.Admin.SearchUsers;
using ClickStitch.Api.Auth;
using ClickStitch.Api.Auth.LogIn;
using ClickStitch.Api.Creators.CreateCreator;
using ClickStitch.Api.Creators.GetCreator;
using ClickStitch.Api.Creators.GetCreatorBySelf;
using ClickStitch.Api.Creators.SearchCreatorPatterns;
using ClickStitch.Api.Creators.UpdateCreator;
using ClickStitch.Api.Inventory.SearchInventoryThreads;
using ClickStitch.Api.Inventory.UpdateInventoryThread;
using ClickStitch.Api.Patterns.CreatePattern;
using ClickStitch.Api.Patterns.CreatePattern.Parsing;
using ClickStitch.Api.Patterns.DeletePattern;
using ClickStitch.Api.Patterns.GetPattern;
using ClickStitch.Api.Patterns.GetPatternInventory;
using ClickStitch.Api.Patterns.SearchPatterns;
using ClickStitch.Api.Patterns.UpdatePattern;
using ClickStitch.Api.Patterns.VerifyPattern;
using ClickStitch.Api.Projects.AddProject;
using ClickStitch.Api.Projects.CompleteBackStitches;
using ClickStitch.Api.Projects.CompleteStitches;
using ClickStitch.Api.Projects.GetAnalytics;
using ClickStitch.Api.Projects.GetProject;
using ClickStitch.Api.Projects.GetProjects;
using ClickStitch.Api.Projects.PauseStitching;
using ClickStitch.Api.Projects.UnCompleteBackStitches;
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
using Data.Repositories.UserPatternThreadBackStitch;
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
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserPatternRepository, UserPatternRepository>();
        services.AddSingleton<IPermissionRepository, PermissionRepository>();
        services.AddSingleton<IThreadRepository, ThreadRepository>();
        services.AddSingleton<IUserCreatorRepository, UserCreatorRepository>();
        services.AddSingleton<IUserPermissionRepository, UserPermissionRepository>();
        services.AddSingleton<IUserPatternThreadBackStitchRepository, UserPatternThreadBackStitchRepository>();
        services.AddSingleton<IUserPatternThreadStitchRepository, UserPatternThreadStitchRepository>();
        services.AddSingleton<IUserThreadRepository, UserThreadRepository>();

        services.AddSingleton<ICloudinaryClient, CloudinaryClient>();

        services.AddSingleton<IAssignPermissionToUserService, AssignPermissionToUserService>();
        services.AddSingleton<IGetPermissionsService, GetPermissionsService>();
        services.AddSingleton<IRemovePermissionFromUserService, RemovePermissionFromUserService>();
        services.AddSingleton<ISearchUsersService, SearchUsersService>();

        services.AddSingleton<ILogInService, LogInService>();
        services.AddSingleton<ILoginTokenService, LoginTokenService>(); 
        services.AddSingleton<IPasswordService, PasswordService>();

        services.AddSingleton<ICreateCreatorService, CreateCreatorService>();
        services.AddSingleton<IGetCreatorService, GetCreatorService>();
        services.AddSingleton<IGetCreatorBySelfService, GetCreatorBySelfService>();
        services.AddSingleton<ISearchCreatorPatternsService, SearchCreatorPatternsService>();
        services.AddSingleton<IUpdateCreatorService, UpdateCreatorService>();

        services.AddSingleton<ISearchInventoryThreadsService, SearchInventoryThreadsService>();
        services.AddSingleton<IUpdateInventoryThreadService, UpdateInventoryThreadService>();

        services.AddSingleton<IDeletePatternService, DeletePatternService>();
        services.AddSingleton<ICreatePatternService, CreatePatternService>();
        services.AddSingleton<IGetPatternService, GetPatternService>();
        services.AddSingleton<IGetPatternInventoryService, GetPatternInventoryService>();
        services.AddSingleton<ISearchPatternsService, SearchPatternsService>();
        services.AddSingleton<IUpdatePatternService, UpdatePatternService>();
        services.AddSingleton<IVerifyPatternService, VerifyPatternService>();

        services.AddSingleton<IPatternUploadService, PatternUploadService>();
        services.AddSingleton<IPatternParserService, PatternParserService>();

        services.AddSingleton<IAddProjectService, AddProjectService>();
        services.AddSingleton<ICompleteBackStitchesService, CompleteBackStitchesService>();
        services.AddSingleton<ICompleteStitchesService, CompleteStitchesService>();
        services.AddSingleton<IGetAnalyticsService, GetAnalyticsService>();
        services.AddSingleton<IGetProjectService, GetProjectService>();
        services.AddSingleton<IGetProjectsService, GetProjectsService>();
        services.AddSingleton<IPauseStitchingService, PauseStitchingService>();
        services.AddSingleton<IUnCompleteBackStitchesService, UnCompleteBackStitchesService>();
        services.AddSingleton<IUnCompleteStitchesService, UnCompleteStitchesService>();
        services.AddSingleton<IUnPauseStitchingService, UnPauseStitchingService>();

        services.AddSingleton<IGetThreadsByColourService, GetThreadsByColourService>();

        services.AddSingleton<ICreateUserService, CreateUserService>();
        services.AddSingleton<IDeleteUserService, DeleteUserService>();
        services.AddSingleton<IGetUserBySelfService, GetUserByUserBySelfService>();
        services.AddSingleton<IUpdateUserService, UpdateUserService>();
    }
}