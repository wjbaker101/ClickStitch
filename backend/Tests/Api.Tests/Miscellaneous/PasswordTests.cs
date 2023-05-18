using ClickStitch.Api.Auth;

namespace Api.Tests.Miscellaneous;

[TestFixture]
[Parallelizable]
public sealed class GivenAPasswordAndSalt
{
    private string _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var subject = new PasswordService();

        _result = subject.Hash("<password>", "<password_salt>");
    }

    [Test]
    public void ThenTheHashedPasswordIsDisplayed()
    {
        Assert.Pass(_result);
    }
}