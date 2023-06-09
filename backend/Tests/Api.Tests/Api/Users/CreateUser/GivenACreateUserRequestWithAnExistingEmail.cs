﻿using ClickStitch.Api.Auth;
using ClickStitch.Api.Users;
using ClickStitch.Api.Users.Types;
using Data.Records;
using Data.Repositories.User;
using Moq;
using TestHelpers.Settings;

namespace Api.Tests.Api.Users.CreateUser;

[TestFixture]
[Parallelizable]
public sealed class GivenACreateUserRequestWithAnExistingEmail
{
    private Mock<IUserRepository> _userRepository = null!;

    private Result<CreateUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _userRepository = new Mock<IUserRepository>();
        _userRepository
            .Setup(mock => mock.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserRecord
            {
                Reference = default,
                CreatedAt = default,
                Email = null!,
                Password = null!,
                PasswordSalt = null!,
                LastLoginAt = null,
                Permissions = new List<PermissionRecord>()
            });

        var subject = new UsersService(
            _userRepository.Object,
            new PasswordService(new TestAppSecrets()),
            FakeGuid.With(Guid.Parse("55993eb0-9824-4dbf-a674-1f5a09205287")),
            FakeDateTime.Default(),
            null!);

        _result = await subject.CreateUser(new CreateUserRequest
        {
            Email = "test@email.com",
            Password = "TestPassword1!"
        }, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectUserIsNotSaved()
    {
        _userRepository.Verify(mock => mock.SaveAsync(It.IsAny<UserRecord>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("Cannot use that email, an existing user already has it. Please try again with a different email."));
    }
}