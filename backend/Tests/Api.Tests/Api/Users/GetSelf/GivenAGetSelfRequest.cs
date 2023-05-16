using ClickStitch.Api.Users;
using ClickStitch.Api.Users.Types;
using Core.Types;
using Data.Records;
using Data.Repositories.User;
using Moq;

namespace Api.Tests.Api.Users.GetSelf;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetSelfRequest
{
    private Result<GetSelfResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(mock => mock.GetByReferenceAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserRecord
            {
                Reference = Guid.Parse("7dafaed7-0c16-41ef-a247-8bf8990d128d"),
                CreatedAt = new DateTime(2023, 05, 01, 16, 39, 14),
                Email = "test@email.com",
                Password = "TestPassword",
                PasswordSalt = "",
                LastLoginAt = null,
                Permissions = new List<PermissionRecord>()
            });

        var subject = new UsersService(userRepository.Object, null!, null!, null!);

        _result = await subject.GetSelf(new RequestUser
        {
            Id = 9371,
            Reference = Guid.Parse("38eba7b8-e53c-4619-a391-7c3d6beff3de")
        }, CancellationToken.None);
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
            Assert.That(user.Reference, Is.EqualTo(Guid.Parse("7dafaed7-0c16-41ef-a247-8bf8990d128d")), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2023, 05, 01, 16, 39, 14)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
        });
    }
}