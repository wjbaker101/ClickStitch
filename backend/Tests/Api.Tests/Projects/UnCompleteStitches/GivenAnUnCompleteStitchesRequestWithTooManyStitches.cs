using ClickStitch.Api.Projects;
using ClickStitch.Api.Projects.Types;
using DotNetLibs.Core.Extensions;

namespace Api.Tests.Projects.UnCompleteStitches;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUnCompleteStitchesRequestWithTooManyStitches
{
    private const int MAX_STITCH_SELECTION = 100;

    private Result<CompleteStitchesResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var stitches = Enumerable
            .Range(0, MAX_STITCH_SELECTION + 1)
            .MapAll(x => new CompleteStitchesRequest.Position { X = x, Y = x });

        var request = new CompleteStitchesRequest
        {
            StitchesByThread = new Dictionary<int, List<CompleteStitchesRequest.Position>>
            {
                [1] = stitches
            }
        };

        var subject = new ProjectsService(null!, null!, null!, null!, null!);

        _result = await subject.UnCompleteStitches(new TestRequestUser(), Guid.Parse("d5925542-128c-40b4-86a1-b8dcddc848f7"), request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("The number of stitches to un-complete exceeds maximum (100), please try again with a smaller selection."));
    }
}