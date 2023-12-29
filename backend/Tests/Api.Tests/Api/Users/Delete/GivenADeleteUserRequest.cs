using ClickStitch.Api.Auth;
using ClickStitch.Api.Users;
using ClickStitch.Api.Users.Types;
using Data.Records;
using Data.Repositories.User;
using DotNetLibs.Core.Services.Fakes;
using Moq;
using TestHelpers.Settings;

namespace Api.Tests.Api.Users.Delete;

[TestFixture]
[Parallelizable]
public sealed class GivenADeleteUserRequest
{
    private UserRecord _user = null!;

    private Mock<IUserRepository> _userRepository = null!;

    private Result<DeleteUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _user = new UserRecord
        {
            Reference = Guid.Parse("85e41403-d6e1-4c50-bf48-50f65713ea53"),
            CreatedAt = new DateTime(2023, 06, 02, 11, 56, 01),
            Email = "test@email.com",
            Password = "TestPassword",
            PasswordSalt = "TestPasswordSalt",
            LastLoginAt = null,
            Permissions = new List<PermissionRecord>()
        };

        _userRepository = new Mock<IUserRepository>();
        _userRepository
            .Setup(mock => mock.GetByReferenceAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_user);

        var subject = new UsersService(
            _userRepository.Object,
            new PasswordService(new TestAppSecrets()),
            new FakeGuidProvider(),
            new FakeDateTimeProvider());
        
        _result = await subject.DeleteUser(new RequestUser
        {
            Id = 1913,
            Reference = Guid.NewGuid(),
            Permissions = new List<RequestPermissionType>()
        }, Guid.Parse("5f69355e-7498-4620-bd6f-cf3968fb37a4"), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectUserIsDeleted()
    {
        _userRepository.Verify(mock => mock.DeleteAsync(_user, It.IsAny<CancellationToken>()), Times.Once);
    }
}