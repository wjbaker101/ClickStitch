﻿using ClickStitch.Api.Users.GetUserBySelf;
using ClickStitch.Api.Users.GetUserBySelf.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Users.GetSelf;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetUserBySelfRequest
{
    private readonly Guid _userReference = Guid.Parse("7c63a2ed-d06d-4b5a-a882-0374b14b6c3a");

    private Result<GetUserBySelfResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                new UserRecord
                {
                    Reference = _userReference,
                    CreatedAt = new DateTime(2023, 05, 01, 16, 39, 14),
                    Email = "test@email.com",
                    Password = "TestPassword",
                    PasswordSalt = "",
                    LastLoginAt = null,
                    Permissions = new List<PermissionRecord>()
                }            }
        };

        var requestUser = new TestRequestUser
        {
            Reference = _userReference
        };

        var subject = new GetUserByUserBySelfService(new UserRepository(database));

        _result = await subject.GetUserBySelf(requestUser, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectUserIsReturned()
    {
        var user = _result.Content.User;

        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(_userReference), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2023, 05, 01, 16, 39, 14)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
        });
    }
}