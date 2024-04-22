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

            Assert.That(pattern.Width, Is.EqualTo(150));
            Assert.That(pattern.Height, Is.EqualTo(150));
            Assert.That(pattern.ThreadCount, Is.EqualTo(2));
            Assert.That(pattern.StitchCount, Is.EqualTo(3));
        });
    }

    [Test]
    public void ThenTheCorrectThreadsAreReturned()
    {
        Assert.Multiple(() =>
        {
            var thread1 = _result.Content.Threads[0];

            Assert.That(thread1.Index, Is.EqualTo(1));
            Assert.That(thread1.Name, Is.EqualTo("DMC B5200"));
            Assert.That(thread1.Description, Is.EqualTo("Snow White"));
            Assert.That(thread1.Colour, Is.EqualTo("#FCFDFD"));

            var thread2 = _result.Content.Threads[1];

            Assert.That(thread2.Index, Is.EqualTo(2));
            Assert.That(thread2.Name, Is.EqualTo("DMC 01"));
            Assert.That(thread2.Description, Is.EqualTo("Tin - White"));
            Assert.That(thread2.Colour, Is.EqualTo("#CBCBCC"));
        });
    }

    [Test]
    public void ThenTheCorrectStitchesAreReturned()
    {
        Assert.Multiple(() =>
        {
            var stitch1 = _result.Content.Stitches[0];

            Assert.That(stitch1.ThreadIndex, Is.EqualTo(1));
            Assert.That(stitch1.X, Is.EqualTo(0));
            Assert.That(stitch1.Y, Is.EqualTo(0));

            var stitch2 = _result.Content.Stitches[1];

            Assert.That(stitch2.ThreadIndex, Is.EqualTo(2));
            Assert.That(stitch2.X, Is.EqualTo(1));
            Assert.That(stitch2.Y, Is.EqualTo(0));

            var stitch3 = _result.Content.Stitches[2];

            Assert.That(stitch3.ThreadIndex, Is.EqualTo(2));
            Assert.That(stitch3.X, Is.EqualTo(2));
            Assert.That(stitch3.Y, Is.EqualTo(0));
        });
    }
}