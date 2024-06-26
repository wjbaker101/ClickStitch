﻿using ClickStitch.Api.Admin.AssignPermissionToUser;
using ClickStitch.Api.Admin.GetPermissions;
using ClickStitch.Api.Admin.RemovePermissionFromUser;
using ClickStitch.Api.Admin.SearchUsers;
using ClickStitch.Api.Auth;
using ClickStitch.Api.Creators.CreateCreator;
using ClickStitch.Api.Creators.GetCreator;
using ClickStitch.Api.Creators.GetCreatorBySelf;
using ClickStitch.Api.Creators.SearchCreatorPatterns;
using ClickStitch.Api.Creators.UpdateCreator;
using ClickStitch.Api.Patterns.CreatePattern;
using ClickStitch.Api.Patterns.DeletePattern;
using ClickStitch.Api.Patterns.GetPattern;
using ClickStitch.Api.Patterns.GetPatternInventory;
using ClickStitch.Api.Patterns.SearchPatterns;
using ClickStitch.Api.Patterns.UpdatePattern;
using ClickStitch.Api.Patterns.VerifyPattern;
using ClickStitch.Models;
using Core.Settings;
using Data.Records;
using Data.Repositories.User;
using Data.Types;
using DotNetLibs.Api.Types;
using DotNetLibs.Core.Services;
using Integration.Tests.Fakes;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using TestHelpers.Data;
using TestHelpers.Settings;

namespace Integration.Tests;

public abstract class IntegrationTest
{
    protected HttpClient Client { get; }

    private static readonly Guid StitcherUserReference = Guid.Parse("aa63c278-ff0c-4e00-86ee-c73f03d976e5");
    private static readonly Guid AdminUserReference = Guid.Parse("c5843ba6-e7ae-4e06-b097-1b3c59c3bb6a");
    private static readonly Guid CreatorUserReference = Guid.Parse("ef4dc33e-1db3-4dd4-a001-d84337e0ebfa");

    protected IntegrationTest()
    {
        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                new UserRecord
                {
                    Id = 0599,
                    Reference = StitcherUserReference,
                    CreatedAt = default,
                    Email = null!,
                    Password = null!,
                    PasswordSalt = null!,
                    LastLoginAt = null,
                    Permissions = new List<PermissionRecord>()
                },
                new UserRecord
                {
                    Id = 5857,
                    Reference = AdminUserReference,
                    CreatedAt = default,
                    Email = null!,
                    Password = null!,
                    PasswordSalt = null!,
                    LastLoginAt = null,
                    Permissions = new List<PermissionRecord>
                    {
                        new()
                        {
                            Type = PermissionType.Admin,
                            Name = "Admin"
                        }
                    }
                },
                new UserRecord
                {
                    Id = 8268,
                    Reference = CreatorUserReference,
                    CreatedAt = default,
                    Email = null!,
                    Password = null!,
                    PasswordSalt = null!,
                    LastLoginAt = null,
                    Permissions = new List<PermissionRecord>
                    {
                        new()
                        {
                            Type = PermissionType.Creator,
                            Name = "Creator"
                        }
                    }
                },
            }
        };

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<AppSecrets>(new TestAppSecrets());
                    services.AddSingleton<IUserRepository>(new UserRepository(database));

                    services.AddSingleton<IDeletePatternService>(new FakeDeletePatternService());
                    services.AddSingleton<ICreatePatternService>(new FakeCreatePatternService());
                    services.AddSingleton<IGetPatternInventoryService>(new FakeGetPatternInventoryService());
                    services.AddSingleton<ISearchPatternsService>(new FakeSearchPatternsService());
                    services.AddSingleton<IGetPatternService>(new FakeGetPatternService());
                    services.AddSingleton<IUpdatePatternService>(new FakeUpdatePatternService());
                    services.AddSingleton<IVerifyPatternService>(new FakeVerifyPatternService());

                    services.AddSingleton<IAssignPermissionToUserService>(new FakeAssignPermissionToUserService());
                    services.AddSingleton<IGetPermissionsService>(new FakeGetPermissionsService());
                    services.AddSingleton<IRemovePermissionFromUserService>(new FakeRemovePermissionFromUserService());
                    services.AddSingleton<ISearchUsersService>(new FakeSearchUsersService());

                    services.AddSingleton<ICreateCreatorService>(new FakeCreateCreatorService());
                    services.AddSingleton<IGetCreatorService>(new FakeGetCreatorService());
                    services.AddSingleton<IGetCreatorBySelfService>(new FakeGetCreatorBySelfService());
                    services.AddSingleton<ISearchCreatorPatternsService>(new FakeSearchCreatorPatternsService());
                    services.AddSingleton<IUpdateCreatorService>(new FakeUpdateCreatorService());
                });
            });

        Client = application.CreateClient();
    }

    private static string CreateLoginToken(Guid userReference)
    {
        return new LoginTokenService(new DateTimeProvider(), new TestAppSecrets()).Create(new UserModel
        {
            Reference = userReference,
            CreatedAt = default,
            Email = null!,
            LastLoginAt = null
        });
    }

    protected void AsStitcher()
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateLoginToken(StitcherUserReference));
    }

    protected void AsAdmin()
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateLoginToken(AdminUserReference));
    }

    protected void AsCreator()
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateLoginToken(CreatorUserReference));
    }

    protected async Task<T> DoRequest<T>(HttpMethod method, [StringSyntax(StringSyntaxAttribute.Uri)] string url)
    {
        var response = await Client.SendAsync(new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(url, UriKind.Relative)
        });

        return await ExpectBody<T>(response.Content);
    }

    private static async Task<T> ExpectBody<T>(HttpContent content)
    {
        var body = await content.ReadAsStringAsync(CancellationToken.None);

        try
        {
            var result = JsonConvert.DeserializeObject<ApiResultResponse<T>>(body);

            if (result == null)
            {
                Assert.Fail("Unable to parse response body. Actual body: " + body);
                return default!;
            }

            return result.Result;
        }
        catch
        {
            Assert.Fail("Unable to parse response body. Actual body: " + body);
            return default!;
        }
    }

    protected async Task<(string Response, HttpStatusCode StatusCode)> DoFailureRequest(HttpMethod method, [StringSyntax(StringSyntaxAttribute.Uri)] string url)
    {
        var response = await Client.SendAsync(new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(url, UriKind.Relative)
        });

        return (await ExpectFailureBody(response.Content), response.StatusCode);
    }

    private static async Task<string> ExpectFailureBody(HttpContent content)
    {
        var body = await content.ReadAsStringAsync(CancellationToken.None);

        try
        {
            var result = JsonConvert.DeserializeObject<ApiErrorResponse>(body);

            if (result == null)
            {
                Assert.Fail("Unable to parse response body. Actual body: " + body);
                return default!;
            }

            return result.ErrorMessage;
        }
        catch
        {
            Assert.Fail("Unable to parse response body. Actual body: " + body);
            return default!;
        }
    }
}