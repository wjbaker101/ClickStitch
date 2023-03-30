using Core.Types;
using CrossStitchViewer.Api.Auth;
using CrossStitchViewer.Api.Users;
using CrossStitchViewer.Api.Users.Types;
using Data.Records;
using Data.Repositories.User;
using Moq;
using TestHelpers.Fakes;
using TestHelpers.Models;

namespace Api.Tests.Api.Users.Delete;

[TestFixture]
[Parallelizable]
public sealed class GivenADeleteUserRequest
{
    private UserRecord _user = null!;

    private Mock<IUserRepository> _userRepository = null!;

    private Result<DeleteUserResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _user = new UserRecord
        {
            Reference = Guid.Parse("85e41403-d6e1-4c50-bf48-50f65713ea53"),
            CreatedAt = new DateTime(2023, 06, 02, 11, 56, 01),
            Email = "test@email.com",
            Username = "TestUsername",
            Password = "TestPassword",
            PasswordSalt = "TestPasswordSalt"
        };

        _userRepository = new Mock<IUserRepository>();
        _userRepository
            .Setup(mock => mock.GetByReference(It.IsAny<Guid>()))
            .Returns(_user);

        var subject = new UsersService(
            _userRepository.Object,
            new PasswordService(),
            FakeGuidService.Default(),
            FakeDateTimeService.Default());
        
        _result = subject.DeleteUser(TestUserModel.Get(), Guid.Parse("5f69355e-7498-4620-bd6f-cf3968fb37a4"));
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectUserIsDeleted()
    {
        _userRepository.Verify(mock => mock.Delete(_user), Times.Once);
    }
}