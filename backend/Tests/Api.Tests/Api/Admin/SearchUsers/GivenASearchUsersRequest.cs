using ClickStitch.Api.Admin;
using ClickStitch.Api.Admin.Types;
using ClickStitch.Models;
using Data.Records;
using Data.Repositories.Admin;
using Data.Repositories.Admin.Types;
using Moq;

namespace Api.Tests.Api.Admin.SearchUsers;

[TestFixture]
[Parallelizable]
public sealed class GivenASearchUsersRequest
{
    private Mock<IAdminRepository> _adminRepository = null!;

    private Result<SearchUsersResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _adminRepository = new Mock<IAdminRepository>();
        _adminRepository
            .Setup(mock => mock.SearchUsers(It.IsAny<SearchUsersParameters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SearchUsersDto
            {
                Users = new List<UserRecord>
                {
                    new()
                    {
                        Reference = Guid.Parse("016fc7d5-03c8-4639-a31d-08ba1fd17ed8"),
                        CreatedAt = new DateTime(2023, 06, 02, 23, 15, 10),
                        Email = "test@email.com",
                        Password = "",
                        PasswordSalt = "",
                        LastLoginAt = new DateTime(2023, 06, 02, 23, 15, 20),
                        Permissions = new List<PermissionRecord>
                        {
                            new()
                            {
                                Type = PermissionType.Creator,
                                Name = "TestPermissionName"
                            }
                        }
                    }
                },
                TotalCount = 4023
            });

        var subject = new AdminService(_adminRepository.Object, FakeUserRepository.Default(), null!, FakeUserPermissionRepository.Default());

        _result = await subject.SearchUsers(2979, 8556, CancellationToken.None);
    }

    [Test]
    public void ThenTheUsersAreRetrieved()
    {
        _adminRepository.Verify(mock => mock.SearchUsers(
            It.Is<SearchUsersParameters>(request =>
                request.PageNumber == 2979 &&
                request.PageSize == 8556),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void ThenTheUserIsMappedCorrectly()
    {
        var user = _result.Content.Users[0].User;

        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(Guid.Parse("016fc7d5-03c8-4639-a31d-08ba1fd17ed8")), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2023, 06, 02, 23, 15, 10)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
            Assert.That(user.LastLoginAt, Is.EqualTo(new DateTime(2023, 06, 02, 23, 15, 20)), nameof(user.LastLoginAt));
        });
    }

    [Test]
    public void ThenThePermissionIsMappedCorrectly()
    {
        var permission = _result.Content.Users[0].Permissions[0];

        Assert.Multiple(() =>
        {
            Assert.That(permission.Type, Is.EqualTo(ApiPermissionType.Creator), nameof(permission.Type));
            Assert.That(permission.Name, Is.EqualTo("TestPermissionName"), nameof(permission.Name));
        });
    }
}