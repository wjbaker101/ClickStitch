using ClickStitch.Api.Auth;
using ClickStitch.Api.Users;
using ClickStitch.Api.Users.Types;
using Core.Types;
using Data.Records;
using Data.Repositories.User;
using Moq;
using TestHelpers.Fakes;

namespace Api.Tests.Api.Users.CreateUser;

[TestFixture]
[Parallelizable]
public sealed class GivenACreateUserRequest
{
    private Mock<IUserRepository> _userRepository = null!;

    private Result<CreateUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _userRepository = new Mock<IUserRepository>();
        _userRepository
            .Setup(mock => mock.SaveAsync(It.IsAny<UserRecord>()))
            .ReturnsAsync((UserRecord user) => user);
        _userRepository
            .Setup(mock => mock.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Result<UserRecord>.Failure("TestFailure"));
        _userRepository
            .Setup(mock => mock.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(Result<UserRecord>.Failure("TestFailure"));

        var subject = new UsersService(
            _userRepository.Object,
            new PasswordService(),
            FakeGuid.With(Guid.Parse("55993eb0-9824-4dbf-a674-1f5a09205287")),
            FakeDateTime.Default());

        _result = await subject.CreateUser(new CreateUserRequest
        {
            Email = "test@email.com",
            Username = "TestUsername",
            Password = "TestPassword1!"
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
        _userRepository.Verify(mock => mock.SaveAsync(It.Is<UserRecord>(request => AssertUserRecord(request))), Times.Once);
    }

    private static bool AssertUserRecord(UserRecord user)
    {
        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(Guid.Parse("55993eb0-9824-4dbf-a674-1f5a09205287")), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2020, 01, 02, 23, 24, 25)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
            Assert.That(user.Username, Is.EqualTo("TestUsername"), nameof(user.Username));
            Assert.That(user.Password, Is.EqualTo("B0E+C2riWQtxPAohh0W08xUa/OzPHAP4Rh5PqfJOY6U="), nameof(user.Password));
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
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2020, 01, 02, 23, 24, 25)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
            Assert.That(user.Username, Is.EqualTo("TestUsername"), nameof(user.Username));
        });
    }
}