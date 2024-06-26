﻿using ClickStitch.Api.Patterns.CreatePattern.Parsing.Types;
using ClickStitch.Api.Patterns.VerifyPattern;
using ClickStitch.Api.Patterns.VerifyPattern.Types;

namespace Api.Tests.Patterns.VerifyPattern;

[TestFixture]
[Parallelizable]
public sealed class GivenAVerifyPatternRequestForAnInvalidPattern
{
    private Result<VerifyPatternResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var patternParserService = new FakePatternParserService
        {
            Response = Result<ParsePatternResponse>.Failure("TestError")
        };

        var subject = new VerifyPatternService(patternParserService);

        _result = subject.VerifyPattern(null!, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("TestError"));
    }
}