using Api.Tests.Patterns.Parsing._Helper;
using ClickStitch.Api.Patterns.CreatePattern.Parsing;
using ClickStitch.Api.Patterns.CreatePattern.Parsing.Types;

namespace Api.Tests.Patterns.Parsing.Default;

[TestFixture]
[Parallelizable]
public sealed class GivenAPatternParseRequestForADefaultSchema
{
    private Result<ParsePatternResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var subject = new PatternParserService(new FakeInkwellClient());

        _result = subject.Parse(new ParsePatternParameters
        {
            RawContent = TestPatternSchemas.FcJson()
        });
    }

    [Test]
    public void ThenTheCorrectPatternIsReturned()
    {
        Assert.Multiple(() =>
        {
            var pattern = _result.Content.Pattern;

            Assert.That(pattern.Width, Is.EqualTo(150), nameof(pattern.Width));
            Assert.That(pattern.Height, Is.EqualTo(150), nameof(pattern.Height));
            Assert.That(pattern.ThreadCount, Is.EqualTo(3), nameof(pattern.ThreadCount));
            Assert.That(pattern.StitchCount, Is.EqualTo(4), nameof(pattern.StitchCount));
        });
    }

    [Test]
    public void ThenTheCorrectThreadsAreReturned()
    {
        var threads = _result.Content.Threads;

        Assert.Multiple(() =>
        {
            Assert.That(threads, Has.Count.EqualTo(3));

            var thread1 = threads[0];

            Assert.That(thread1.Index, Is.EqualTo(1));
            Assert.That(thread1.Name, Is.EqualTo("DMC B5200"));
            Assert.That(thread1.Description, Is.EqualTo("Snow White"));
            Assert.That(thread1.Colour, Is.EqualTo("#FCFDFD"));

            var thread2 = threads[1];

            Assert.That(thread2.Index, Is.EqualTo(2));
            Assert.That(thread2.Name, Is.EqualTo("DMC 444"));
            Assert.That(thread2.Description, Is.EqualTo("Lemon - Dark"));
            Assert.That(thread2.Colour, Is.EqualTo("#F9D62E"));

            var thread3 = threads[2];

            Assert.That(thread3.Index, Is.EqualTo(3));
            Assert.That(thread3.Name, Is.EqualTo("DMC 01"));
            Assert.That(thread3.Description, Is.EqualTo("Tin - White"));
            Assert.That(thread3.Colour, Is.EqualTo("#CBCBCC"));
        });
    }

    [Test]
    public void ThenTheCorrectStitchesAreReturned()
    {
        Assert.Multiple(() =>
        {
            var stitch1 = _result.Content.Stitches[0];

            Assert.That(stitch1.ThreadIndex, Is.EqualTo(1));
            Assert.That(stitch1.X, Is.EqualTo(1));
            Assert.That(stitch1.Y, Is.EqualTo(0));

            var stitch2 = _result.Content.Stitches[1];

            Assert.That(stitch2.ThreadIndex, Is.EqualTo(2));
            Assert.That(stitch2.X, Is.EqualTo(2));
            Assert.That(stitch2.Y, Is.EqualTo(0));

            var stitch3 = _result.Content.Stitches[2];

            Assert.That(stitch3.ThreadIndex, Is.EqualTo(2));
            Assert.That(stitch3.X, Is.EqualTo(4));
            Assert.That(stitch3.Y, Is.EqualTo(0));
        });
    }

    [Test]
    public void ThenTheCorrectBackStitchesAreReturned()
    {
        Assert.Multiple(() =>
        {
            var backStitch = _result.Content.BackStitches[0];

            Assert.That(backStitch.ThreadIndex, Is.EqualTo(3), nameof(backStitch.ThreadIndex));
            Assert.That(backStitch.StartX, Is.EqualTo(7), nameof(backStitch.StartX));
            Assert.That(backStitch.StartY, Is.EqualTo(3), nameof(backStitch.StartY));
            Assert.That(backStitch.EndX, Is.EqualTo(6), nameof(backStitch.EndX));
            Assert.That(backStitch.EndY, Is.EqualTo(5), nameof(backStitch.EndY));
        });
    }
}