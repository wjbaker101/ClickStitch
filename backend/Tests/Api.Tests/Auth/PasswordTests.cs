using ClickStitch.Api.Auth;
using TestHelpers.Settings;

namespace Api.Tests.Auth;

[TestFixture]
[Parallelizable]
public sealed class GivenAPasswordAndSalt
{
    private string _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var subject = new PasswordService(new TestAppSecrets());

        _result = subject.Hash("<password>", "<password_salt>");
    }

    [Test]
    public void ThenTheHashedPasswordIsDisplayed()
    {
        Assert.Pass(_result);
    }
}