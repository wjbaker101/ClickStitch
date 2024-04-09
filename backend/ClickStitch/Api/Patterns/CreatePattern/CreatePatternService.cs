using ClickStitch.Api.Patterns.CreatePattern.Types;
using ClickStitch.Api.Patterns.VerifyPattern.Parsing;
using ClickStitch.Api.Patterns.VerifyPattern.Parsing.Types;
using ClickStitch.Services;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using DotNetLibs.Core.Services;

namespace ClickStitch.Api.Patterns.CreatePattern;

public interface ICreatePatternService
{
    Task<Result> CreatePattern(RequestUser requestUser, CreatePatternRequest request, string patternData, IFormFile thumbnail, IFormFile? bannerImage, CancellationToken cancellationToken);
}

public sealed class CreatePatternService : ICreatePatternService
{
    private readonly IPatternRepository _patternRepository;
    private readonly IPatternThreadRepository _patternThreadRepository;
    private readonly IPatternUploadService _patternUploadService;
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly ICreatorRepository _creatorRepository;
    private readonly IPatternThreadStitchRepository _patternThreadStitchRepository;
    private readonly IPatternParserService _patternParserService;
    private readonly IGuidProvider _guidProvider;

    public CreatePatternService(
        IPatternRepository patternRepository,
        IPatternThreadRepository patternThreadRepository,
        IPatternUploadService patternUploadService,
        IUserRepository userRepository,
        IUserPatternRepository userPatternRepository,
        ICreatorRepository creatorRepository,
        IPatternThreadStitchRepository patternThreadStitchRepository,
        IPatternParserService patternParserService,
        IGuidProvider guidProvider)
    {
        _patternRepository = patternRepository;
        _patternThreadRepository = patternThreadRepository;
        _patternUploadService = patternUploadService;
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _creatorRepository = creatorRepository;
        _patternThreadStitchRepository = patternThreadStitchRepository;
        _patternParserService = patternParserService;
        _guidProvider = guidProvider;
    }

    public async Task<Result> CreatePattern(
        RequestUser requestUser,
        CreatePatternRequest request,
        string patternData,
        IFormFile thumbnail,
        IFormFile? bannerImage,
        CancellationToken cancellationToken)
    {
        var titleSlugResult = SlugService.Generate(request.Title);
        if (!titleSlugResult.TrySuccess(out var titleSlug))
            return Result.FromFailure(titleSlugResult);

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var parseResult = _patternParserService.Parse(new ParsePatternParameters
        {
            RawContent = patternData
        });
        if (!parseResult.TrySuccess(out var parsed))
            return Result.FromFailure(parseResult);

        var bannerImageStream = bannerImage != null ? bannerImage.OpenReadStream() : PatternThumbnailGenerator.Create(parsed.Pattern.Width, parsed.Pattern.Height, parsed.Threads, parsed.Stitches);
        var patternReference = _guidProvider.NewGuid();

        var bannerUrlResult = await _patternUploadService.UploadImage(patternReference.ToString(), PatternImageType.Banner, bannerImageStream, cancellationToken);
        if (bannerUrlResult.IsFailure)
            return Result.FromFailure(bannerUrlResult);

        var isCreator = requestUser.Permissions.IsCreator();

        CreatorRecord? creator = null;
        if (isCreator)
        {
            var creatorResult = await _creatorRepository.GetByUser(user, cancellationToken);
            if (creatorResult.IsFailure)
                return Result.FromFailure(creatorResult);

            creator = creatorResult.Content;
        }

        var pattern = await _patternRepository.SaveAsync(new PatternRecord
        {
            Reference = patternReference,
            CreatedAt = DateTime.UtcNow,
            Title = request.Title,
            Width = parsed.Pattern.Width,
            Height = parsed.Pattern.Height,
            Price = request.Price,
            ThumbnailUrl = "",
            ThreadCount = parsed.Pattern.ThreadCount,
            StitchCount = parsed.Pattern.StitchCount,
            AidaCount = request.AidaCount,
            BannerImageUrl = bannerUrlResult.Content,
            ExternalShopUrl = request.ExternalShopUrl,
            TitleSlug = titleSlug,
            IsPublic = isCreator,
            User = user,
            Creator = creator,
            Threads = new HashSet<PatternThreadRecord>()
        }, cancellationToken);

        var threads = await _patternThreadRepository.SaveManyAsync(parsed.Threads.ConvertAll(x => new PatternThreadRecord
        {
            Pattern = pattern,
            Name = x.Name,
            Description = x.Description,
            Index = x.Index,
            Colour = x.Colour,
            Stitches = parsed.Stitches.Where(stitch => stitch.ThreadIndex == x.Index).Select(stitch => new [] { stitch.X, stitch.Y }).ToArray()
        }), cancellationToken);

        var threadLookup = threads.ToDictionary(x => x.Index);

        await _patternThreadStitchRepository.SaveAll(parsed.Stitches.ConvertAll(x => new PatternThreadStitchRecord
        {
            Thread = threadLookup[x.ThreadIndex],
            X = x.X,
            Y = x.Y
        }), cancellationToken);

        if (!isCreator)
        {
            await _userPatternRepository.SaveAsync(new UserPatternRecord
            {
                User = user,
                Pattern = pattern,
                Reference = _guidProvider.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                PausePositionX = null,
                PausePositionY = null
            }, cancellationToken);
        }

        return Result.Success();
    }
}