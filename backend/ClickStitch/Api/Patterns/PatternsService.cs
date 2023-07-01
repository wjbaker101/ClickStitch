using ClickStitch.Api.Patterns.Types;
using Core.Extensions;
using Core.Services;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.Pattern;
using Data.Repositories.Pattern.Types;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace ClickStitch.Api.Patterns;

public interface IPatternsService
{
    Task<Result<GetPatternsResponse>> GetPatterns(RequestUser? requestUser, CancellationToken cancellationToken);
    Task<Result<UpdatePatternResponse>> UpdatePattern(RequestUser requestUser, Guid patternReference, UpdatePatternRequest request, CancellationToken cancellationToken);
    Task<Result> CreatePattern(RequestUser requestUser, CreatePatternRequest request, CreatePatternData patternData, IFormFile thumbnail, IFormFile bannerImage, CancellationToken cancellationToken);
}

public sealed class PatternsService : IPatternsService
{
    private readonly IPatternRepository _patternRepository;
    private readonly IPatternStitchRepository _patternStitchRepository;
    private readonly IPatternThreadRepository _patternThreadRepository;
    private readonly IPatternUploadService _patternUploadService;
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly ICreatorRepository _creatorRepository;
    private readonly IPatternThreadStitchRepository _patternThreadStitchRepository;

    public PatternsService(
        IPatternRepository patternRepository,
        IPatternStitchRepository patternStitchRepository,
        IPatternThreadRepository patternThreadRepository,
        IPatternUploadService patternUploadService,
        IUserRepository userRepository,
        IUserPatternRepository userPatternRepository,
        ICreatorRepository creatorRepository,
        IPatternThreadStitchRepository patternThreadStitchRepository)
    {
        _patternRepository = patternRepository;
        _patternStitchRepository = patternStitchRepository;
        _patternThreadRepository = patternThreadRepository;
        _patternUploadService = patternUploadService;
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _creatorRepository = creatorRepository;
        _patternThreadStitchRepository = patternThreadStitchRepository;
    }

    public async Task<Result<GetPatternsResponse>> GetPatterns(RequestUser? requestUser, CancellationToken cancellationToken)
    {
        var patternsToExclude = new List<PatternRecord>();

        if (requestUser != null)
        {
            var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

            var projects = await _userPatternRepository.GetByUserAsync(user, cancellationToken);

            patternsToExclude.AddRange(projects.ConvertAll(x => x.Pattern));
        }

        var patterns = await _patternRepository.SearchAsync(new SearchPatternsParameters
        {
            PatternsToExclude = patternsToExclude
        }, cancellationToken);

        return new GetPatternsResponse
        {
            Patterns = patterns.ConvertAll(PatternMapper.MapWithCreator)
        };
    }

    public async Task<Result<UpdatePatternResponse>> UpdatePattern(
        RequestUser requestUser,
        Guid patternReference,
        UpdatePatternRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var creatorResult = await _creatorRepository.GetByUser(user, cancellationToken);
        if (!creatorResult.TrySuccess(out var creator))
            return Result<UpdatePatternResponse>.FromFailure(creatorResult);

        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<UpdatePatternResponse>.FromFailure(patternResult);

        if (pattern.Creator.Id != creator.Id)
            return Result<UpdatePatternResponse>.Failure("Unable to update pattern as you are not a creator of it.");

        pattern.Title = request.Title;
        pattern.ExternalShopUrl = request.ExternalShopUrl;

        await _patternRepository.UpdateAsync(pattern, cancellationToken);

        return new UpdatePatternResponse
        {
            Pattern = PatternMapper.MapWithoutCreator(pattern)
        };
    }

    public async Task<Result> CreatePattern(
        RequestUser requestUser,
        CreatePatternRequest request,
        CreatePatternData patternData,
        IFormFile thumbnail,
        IFormFile bannerImage,
        CancellationToken cancellationToken)
    {
        if (requestUser.Permissions.All(x => x != RequestPermissionType.Creator))
            return Result.Failure("You cannot create patterns if you are not a creator.");

        var thumbnailUrlResult = await _patternUploadService.UploadImage(request.ImageFileName, PatternImageType.Thumbnail, thumbnail.OpenReadStream(), cancellationToken);
        if (thumbnailUrlResult.IsFailure)
            return Result.FromFailure(thumbnailUrlResult);

        var bannerUrlResult = await _patternUploadService.UploadImage(request.ImageFileName, PatternImageType.Banner, bannerImage.OpenReadStream(), cancellationToken);
        if (bannerUrlResult.IsFailure)
            return Result.FromFailure(bannerUrlResult);

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var creatorResult = await _creatorRepository.GetByUser(user, cancellationToken);
        if (creatorResult.IsFailure)
            return Result.FromFailure(creatorResult);

        var slugResult = SlugService.Generate(request.Title);
        if (slugResult.IsFailure)
            return Result.FromFailure(slugResult);

        var pattern = await _patternRepository.SaveAsync(new PatternRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Title = request.Title,
            Width = patternData.canvas.width,
            Height = patternData.canvas.height,
            Price = request.Price,
            ThumbnailUrl = thumbnailUrlResult.Content,
            ThreadCount = patternData.palette.threads.Count - 1,
            StitchCount = patternData.canvas.stitches.Count,
            AidaCount = request.AidaCount,
            BannerImageUrl = bannerUrlResult.Content,
            ExternalShopUrl = null,
            Creator = creatorResult.Content,
            TitleSlug = slugResult.Content,
            Stitches = new HashSet<PatternStitchRecord>(),
            Threads = new HashSet<PatternThreadRecord>()
        }, cancellationToken);

        var threads = await _patternThreadRepository.SaveManyAsync(patternData.palette.threads
            .Where(x => x.index != 0)
            .MapAll(x => new PatternThreadRecord
            {
                Pattern = pattern,
                Name = x.name,
                Description = x.description,
                Index = x.index,
                Colour = $"#{x.colour.ToLower()}"
            }), cancellationToken);

        var stitchesByThread = patternData.canvas.stitches
            .GroupBy(x => x.index)
            .ToDictionary(x => x.Key, x => x.ToList());

        foreach (var thread in threads)
        {
            var stitches = stitchesByThread[thread.Index];

            await _patternThreadStitchRepository.SaveManyAsync(stitches.ConvertAll(x => new PatternThreadStitchRecord
            {
                Thread = thread,
                X = x.x,
                Y = x.y
            }), cancellationToken);
        }

        return Result.Success();
    }
}