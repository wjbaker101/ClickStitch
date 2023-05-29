using ClickStitch.Api.Creators;
using ClickStitch.Api.Creators.Types;

namespace Api.Tests.Api.Creators.CreateCreator;

[TestFixture]
[Parallelizable]
public sealed class GivenACreateCreatorRequest
{
    private Result<CreateCreatorResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var subject = new CreatorsService(FakeCreatorRepository.Default(), null!, null!, null!, null!);

        _result = await subject.CreateCreator(new TestRequestUser(), new CreateCreatorRequest
        {
            Name = "TestName",
            StoreUrl = "TestStoreUrl"
        }, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectCreatorIsReturned()
    {
        var creator = _result.Content.Creator;

        Assert.Multiple(() =>
        {
            Assert.That(creator.Reference, Is.Not.EqualTo(default(Guid)), nameof(creator.Reference));
            Assert.That(creator.CreatedAt, Is.Not.EqualTo(default(DateTime)), nameof(creator.CreatedAt));
            Assert.That(creator.Name, Is.EqualTo("TestName"), nameof(creator.Name));
            Assert.That(creator.StoreUrl, Is.EqualTo("TestStoreUrl"), nameof(creator.StoreUrl));
        });
    }
}