using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.Parsing.Types;
using Data.Repositories.Pattern;
using Data.Repositories.UserPattern;
using DotNetLibs.Core.Extensions;

namespace ClickStitch.Api.Share;

public interface IShareService
{
    Task<Result<Stream>> ShareProject(Guid projectReference, CancellationToken cancellationToken);
}

public sealed class ShareService : IShareService
{
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly IPatternRepository _patternRepository;

    public ShareService(IUserPatternRepository userPatternRepository, IPatternRepository patternRepository)
    {
        _userPatternRepository = userPatternRepository;
        _patternRepository = patternRepository;
    }

    public async Task<Result<Stream>> ShareProject(Guid projectReference, CancellationToken cancellationToken)
    {
        var projectResult = await _userPatternRepository.GetByReference(projectReference, cancellationToken);
        if (!projectResult.TrySuccess(out var project))
            return Result<Stream>.FromFailure(projectResult);

        var patternResult = await _patternRepository.GetWithThreadsByReferenceAsync(project.Pattern.Reference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<Stream>.FromFailure(patternResult);

        var stitches = await _patternRepository.GetStitchesByThreads(pattern.Threads.ToList(), cancellationToken);

        var stitchesOfThreads = pattern.Threads.MapAll(x => new ParsePatternResponse.ThreadDetails
        {
            Name = x.Name,
            Description = x.Description,
            Index = x.Index,
            Colour = x.Colour
        });

        var stitchDetails = new List<ParsePatternResponse.StitchDetails>();
        foreach (var stitchesOfThread in stitches.Values)
        {
            stitchDetails.AddRange(stitchesOfThread.ConvertAll(x => new ParsePatternResponse.StitchDetails
            {
                ThreadIndex = x.Thread.Index,
                X = x.X,
                Y = x.Y
            }));
        }

        var asd = PatternThumbnailGenerator.Create(pattern.Width, pattern.Height, stitchesOfThreads, stitchDetails);

        return asd;
    }
}