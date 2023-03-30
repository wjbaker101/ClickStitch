using Core.Types;
using CrossStitchViewer.Api.Auth;
using CrossStitchViewer.Api.Users;
using CrossStitchViewer.Api.Users.Types;
using Data.Records;
using Data.Repositories.User;
using Moq;
using TestHelpers.Fakes;

namespace Api.Tests.Api.Users.Create;

[TestFixture]
[Parallelizable]
public sealed class GivenACreateUserRequest
{
    private Mock<IUserRepository> _userRepository = null!;

    private Result<CreateUserResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _userRepository = new Mock<IUserRepository>();
        _userRepository
            .Setup(mock => mock.Save(It.IsAny<UserRecord>()))
            .Returns((UserRecord user) => user);

        var subject = new UsersService(
            _userRepository.Object,
            new PasswordService(),
            FakeGuidService.With(Guid.Parse("55993eb0-9824-4dbf-a674-1f5a09205287")),
            FakeDateTimeService.With(new DateTime(2023, 04, 27, 18, 22, 58)));

        _result = subject.CreateUser(new CreateUserRequest
        {
            Email = "test@email.com",
            Username = "TestUsername",
            Password = "TestPassword"
        });
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectUserIsSaved()
    {
        _userRepository.Verify(mock => mock.Save(It.Is<UserRecord>(request => AssertUserRecord(request))), Times.Once);
    }

    private static bool AssertUserRecord(UserRecord user)
    {
        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(Guid.Parse("55993eb0-9824-4dbf-a674-1f5a09205287")), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2023, 04, 27, 18, 22, 58)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
            Assert.That(user.Username, Is.EqualTo("TestUsername"), nameof(user.Username));
            Assert.That(user.Password, Is.EqualTo("gAPyiuD6HF4UdLEAxv5Cr9qW3goqMMp7vU98IXG2eQ0="), nameof(user.Password));
            Assert.That(user.PasswordSalt, Is.EqualTo("55993eb0-9824-4dbf-a674-1f5a09205287"), nameof(user.PasswordSalt));
        });

        return true;
    }

    [Test]
    public void ThenTheCorrectUserIsReturned()
    {
        var user = _result.Content.User;
        
        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(Guid.Parse("55993eb0-9824-4dbf-a674-1f5a09205287")), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2023, 04, 27, 18, 22, 58)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
            Assert.That(user.Username, Is.EqualTo("TestUsername"), nameof(user.Username));
        });
    }
}