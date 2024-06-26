﻿using ClickStitch.Api.Users.UpdateUser;
using ClickStitch.Api.Users.UpdateUser.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Users.Update;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUpdateUserRequest
{
    private readonly Guid _userReference = Guid.Parse("85e41403-d6e1-4c50-bf48-50f65713ea53");

    private TestDatabase _database = null!;

    private Result<UpdateUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                new UserRecord
                {
                    Reference = _userReference,
                    CreatedAt = new DateTime(2023, 06, 02, 11, 56, 01),
                    Email = "test@email.com",
                    Password = "TestPassword",
                    PasswordSalt = "TestPasswordSalt",
                    LastLoginAt = null,
                    Permissions = new List<PermissionRecord>()
                }
            }
        };

        var requestUser = new TestRequestUser
        {
            Reference = _userReference
        };

        var subject = new UpdateUserService(new UserRepository(_database));

        _result = await subject.UpdateUser(requestUser, _userReference, new UpdateUserRequest(), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectUserIsUpdated()
    {
        var user = _database.Actions.Updated.OfType<UserRecord>().Single();

        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(_userReference), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2023, 06, 02, 11, 56, 01)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
            Assert.That(user.Password, Is.EqualTo("TestPassword"), nameof(user.Password));
            Assert.That(user.PasswordSalt, Is.EqualTo("TestPasswordSalt"), nameof(user.PasswordSalt));
        });
    }

    [Test]
    public void ThenTheCorrectUserIsReturned()
    {
        var user = _result.Content.User;

        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(_userReference), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2023, 06, 02, 11, 56, 01)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
        });
    }
}