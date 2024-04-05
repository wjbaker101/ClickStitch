using ClickStitch.Api.Auth;
using ClickStitch.Api.Users.CreateUser;
using ClickStitch.Api.Users.CreateUser.Types;
using Data.Records;
using Data.Repositories.User;
using DotNetLibs.Core.Services.Fakes;
using TestHelpers.Data;
using TestHelpers.Settings;

namespace Api.Tests.Users.CreateUser;

[TestFixture]
[Parallelizable]
public sealed class GivenACreateUserRequest
{
    private TestDatabase _database = null!;

    private Result<CreateUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _database = new TestDatabase();

        var subject = new CreateUserService(
            new UserRepository(_database),
            new PasswordService(new TestAppSecrets()),
            new FakeGuidProvider
            {
                FakeGuid = Guid.Parse("55993eb0-9824-4dbf-a674-1f5a09205287")
            },
            new FakeDateTimeProvider());

        _result = await subject.CreateUser(new CreateUserRequest
        {
            Email = "test@email.com ",
            Password = "TestPassword1!"
        }, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectUserIsSaved()
    {
        var user = _database.Actions.Saved.OfType<UserRecord>().Single();

        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(Guid.Parse("55993eb0-9824-4dbf-a674-1f5a09205287")), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2012, 04, 11, 04, 16, 58)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
            Assert.That(user.Password, Is.EqualTo("/y4uCdYgHfFU4jGJNBrTk1waTQS8g8gR0UnOdiRo5OY="), nameof(user.Password));
            Assert.That(user.PasswordSalt, Is.EqualTo("55993eb0-9824-4dbf-a674-1f5a09205287"), nameof(user.PasswordSalt));
        });
    }

    [Test]
    public void ThenTheCorrectUserIsReturned()
    {
        var user = _result.Content.User;

        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(Guid.Parse("55993eb0-9824-4dbf-a674-1f5a09205287")), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2012, 04, 11, 04, 16, 58)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
        });
    }
}