﻿using ClickStitch.Api.Auth;
using ClickStitch.Api.Users;
using ClickStitch.Api.Users.Types;
using Data.Records;
using Data.Repositories.User;
using Moq;
using TestHelpers.Settings;

namespace Api.Tests.Api.Users.Update;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUpdateUserRequest
{
    private Mock<IUserRepository> _userRepository = null!;

    private Result<UpdateUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _userRepository = new Mock<IUserRepository>();
        _userRepository
            .Setup(mock => mock.GetByReferenceAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserRecord
            {
                Reference = Guid.Parse("85e41403-d6e1-4c50-bf48-50f65713ea53"),
                CreatedAt = new DateTime(2023, 06, 02, 11, 56, 01),
                Email = "test@email.com",
                Password = "TestPassword",
                PasswordSalt = "TestPasswordSalt",
                LastLoginAt = null,
                Permissions = new List<PermissionRecord>()
            });
        _userRepository
            .Setup(mock => mock.UpdateAsync(It.IsAny<UserRecord>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserRecord user, CancellationToken cancellationToken) => user);

        var subject = new UsersService(
            _userRepository.Object,
            new PasswordService(new TestAppSecrets()),
            FakeGuid.Default(),
            FakeDateTime.Default(),
            null!);
        
        _result = await subject.UpdateUser(new RequestUser
        {
            Id = 6713,
            Reference = Guid.NewGuid(),
            Permissions = new List<RequestPermissionType>()
        }, Guid.Parse("5f69355e-7498-4620-bd6f-cf3968fb37a4"), new UpdateUserRequest
        {
        }, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectUserIsUpdated()
    {
        _userRepository.Verify(mock => mock.UpdateAsync(It.Is<UserRecord>(request => AssertUserRecord(request)), It.IsAny<CancellationToken>()), Times.Once);
    }

    private static bool AssertUserRecord(UserRecord user)
    {
        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(Guid.Parse("85e41403-d6e1-4c50-bf48-50f65713ea53")), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2023, 06, 02, 11, 56, 01)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
            Assert.That(user.Password, Is.EqualTo("TestPassword"), nameof(user.Password));
            Assert.That(user.PasswordSalt, Is.EqualTo("TestPasswordSalt"), nameof(user.PasswordSalt));
        });

        return true;
    }

    [Test]
    public void ThenTheCorrectUserIsReturned()
    {
        var user = _result.Content.User;
        
        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(Guid.Parse("85e41403-d6e1-4c50-bf48-50f65713ea53")), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2023, 06, 02, 11, 56, 01)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
        });
    }
}