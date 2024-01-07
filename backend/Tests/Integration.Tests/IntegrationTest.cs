using ClickStitch.Api.Auth;
using ClickStitch.Api.Patterns;
using ClickStitch.Models;
using ClickStitch.Types;
using Core.Settings;
using Data.Repositories.User;
using DotNetLibs.Core.Services;
using Integration.Tests.Fakes;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http.Headers;
using TestHelpers.Settings;
using FakeUserRepository = Integration.Tests.Fakes.FakeUserRepository;

namespace Integration.Tests;

public abstract class IntegrationTest
{
    private enum UserType
    {
        None,
        Stitcher,
        Admin,
        Creator
    }

    protected HttpClient Client { get; }

    private UserType _userType = UserType.None;

    protected IntegrationTest()
    {
        var loginTokenService = new LoginTokenService(new DateTimeProvider(), new TestAppSecrets());
        string? loginToken = null;

        if (_userType == UserType.Stitcher)
        {
            loginTokenService.Create(new UserModel
            {
                Reference = Guid.Parse("aa63c278-ff0c-4e00-86ee-c73f03d976e5"),
                CreatedAt = default,
                Email = null,
                LastLoginAt = null
            });
        }

        if (_userType == UserType.Admin)
        {
            loginTokenService.Create(new UserModel
            {
                Reference = Guid.Parse("c5843ba6-e7ae-4e06-b097-1b3c59c3bb6a"),
                CreatedAt = default,
                Email = null,
                LastLoginAt = null
            });
        }

        if (_userType == UserType.Creator)
        {
            loginTokenService.Create(new UserModel
            {
                Reference = Guid.Parse("ef4dc33e-1db3-4dd4-a001-d84337e0ebfa"),
                CreatedAt = default,
                Email = null,
                LastLoginAt = null
            });
        }

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<AppSecrets>(new TestAppSecrets());
                    services.AddSingleton<IUserRepository>(new FakeUserRepository());
                    services.AddSingleton<IPatternsService>(new FakePatternsService());
                });
            });

        Client = application.CreateClient();

        if (loginToken != null)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginToken);
        }
    }

    protected void AsStitcher()
    {
        _userType = UserType.Stitcher;
    }

    protected void AsAdmin()
    {
        _userType = UserType.Admin;
    }

    protected void AsCreator()
    {
        _userType = UserType.Creator;
    }

    protected async Task<T?> ExpectBody<T>(HttpContent content)
    {
        var body = await content.ReadAsStringAsync(CancellationToken.None);

        try
        {
            var result = JsonConvert.DeserializeObject<ApiResultResponse<T>>(body);

            if (result == null)
            {
                Assert.Fail("Unable to parse response body. Actual body: " + body);
                return default;
            }

            return result.Result;
        }
        catch
        {
            Assert.Fail("Unable to parse response body. Actual body: " + body);
            return default;
        }
    }
}