using ClickStitch.Api.Projects;
using ClickStitch.Api.Projects.Types;

namespace Api.Tests.Api.Projects;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetProjectsRequest
{
    private Result<GetProjectsResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var subject = new ProjectsService(FakeUserRepository.Default(), FakeUserPatternRepository.Default(), null!, null!, null!);

        _result = await subject.GetProjects(new TestRequestUser(), CancellationToken.None);
    }

    [Test]
    public void ThenThePatternIsMappedCorrectly()
    {
        var project = _result.Content.Projects[0];
        var pattern = project.Pattern;

        Assert.Multiple(() =>
        {
            Assert.That(project.PurchasedAt, Is.EqualTo(new DateTime(2023, 12, 07, 11, 19, 00)), nameof(project.PurchasedAt));

            Assert.That(pattern.Reference, Is.EqualTo(Guid.Parse("571099c0-be56-4a49-8dd6-1fb77c95cb04")), nameof(pattern.Reference));
            Assert.That(pattern.CreatedAt, Is.EqualTo(new DateTime(2023, 12, 12, 01, 33, 57)), nameof(pattern.CreatedAt));
            Assert.That(pattern.Title, Is.EqualTo("TestTitle"), nameof(pattern.Title));
            Assert.That(pattern.Width, Is.EqualTo(8773), nameof(pattern.Width));
            Assert.That(pattern.Height, Is.EqualTo(7725), nameof(pattern.Height));
            Assert.That(pattern.Price, Is.EqualTo(884.23m), nameof(pattern.Price));
            Assert.That(pattern.ThumbnailUrl, Is.EqualTo("TestThumbnailUrl"), nameof(pattern.ThumbnailUrl));
            Assert.That(pattern.ThreadCount, Is.EqualTo(6897), nameof(pattern.ThreadCount));
            Assert.That(pattern.StitchCount, Is.EqualTo(1446), nameof(pattern.StitchCount));
            Assert.That(pattern.BannerImageUrl, Is.EqualTo("TestBannerImageUrl"), nameof(pattern.BannerImageUrl));
            Assert.That(pattern.ExternalShopUrl, Is.EqualTo("TestExternalShopUrl"), nameof(pattern.ExternalShopUrl));
        });
    }
}