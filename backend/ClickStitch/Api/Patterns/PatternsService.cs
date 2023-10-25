using ClickStitch.Api.Patterns.Parsing;
using ClickStitch.Api.Patterns.Parsing.Types;
using ClickStitch.Api.Patterns.Types;
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
    Task<Result> CreatePattern(RequestUser requestUser, CreatePatternRequest request, string patternData, IFormFile thumbnail, IFormFile bannerImage, CancellationToken cancellationToken);
    Task<Result<VerifyPatternResponse>> VerifyPattern(string patternData, CancellationToken cancellationToken);
    Task<Result<DeletePatternResponse>> DeletePattern(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
}

public sealed class PatternsService : IPatternsService
{
    private readonly IPatternRepository _patternRepository;
    private readonly IPatternThreadRepository _patternThreadRepository;
    private readonly IPatternUploadService _patternUploadService;
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly ICreatorRepository _creatorRepository;
    private readonly IPatternThreadStitchRepository _patternThreadStitchRepository;
    private readonly IPatternParserService _patternParserService;

    public PatternsService(
        IPatternRepository patternRepository,
        IPatternThreadRepository patternThreadRepository,
        IPatternUploadService patternUploadService,
        IUserRepository userRepository,
        IUserPatternRepository userPatternRepository,
        ICreatorRepository creatorRepository,
        IPatternThreadStitchRepository patternThreadStitchRepository,
        IPatternParserService patternParserService)
    {
        _patternRepository = patternRepository;
        _patternThreadRepository = patternThreadRepository;
        _patternUploadService = patternUploadService;
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _creatorRepository = creatorRepository;
        _patternThreadStitchRepository = patternThreadStitchRepository;
        _patternParserService = patternParserService;
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
        string patternData,
        IFormFile thumbnail,
        IFormFile bannerImage,
        CancellationToken cancellationToken)
    {
        var titleSlugResult = SlugService.Generate(request.Title);
        if (!titleSlugResult.TrySuccess(out var titleSlug))
            return Result.FromFailure(titleSlugResult);

        var bannerUrlResult = await _patternUploadService.UploadImage(titleSlug, PatternImageType.Banner, bannerImage.OpenReadStream(), cancellationToken);
        if (bannerUrlResult.IsFailure)
            return Result.FromFailure(bannerUrlResult);

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var creatorResult = await _creatorRepository.GetByUser(user, cancellationToken);
        if (creatorResult.IsFailure)
            return Result.FromFailure(creatorResult);

        var parseResult = _patternParserService.Parse(new ParsePatternParameters
        {
            RawContent = patternData
        });
        if (!parseResult.TrySuccess(out var parsed))
            return Result.FromFailure(parseResult);

        var pattern = await _patternRepository.SaveAsync(new PatternRecord
        {
            Reference = Guid.NewGuid(),
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
            Creator = creatorResult.Content,
            TitleSlug = titleSlug,
            IsPublic = requestUser.Permissions.Any(x => x == RequestPermissionType.Creator),
            Threads = new HashSet<PatternThreadRecord>()
        }, cancellationToken);

        var threads = await _patternThreadRepository.SaveManyAsync(parsed.Threads.ConvertAll(x => new PatternThreadRecord
        {
            Pattern = pattern,
            Name = x.Name,
            Description = x.Description,
            Index = x.Index,
            Colour = x.Colour
        }), cancellationToken);

        var threadLookup = threads.ToDictionary(x => x.Index);

        var stitches = await _patternThreadStitchRepository.SaveManyAsync(parsed.Stitches.ConvertAll(x => new PatternThreadStitchRecord
        {
            Thread = threadLookup[x.ThreadIndex],
            X = x.X,
            Y = x.Y,
            LookupHash = $"{x.X},{x.Y}"
        }), cancellationToken);

        var isStitcher = requestUser.Permissions.All(x => x != RequestPermissionType.Creator);
        if (isStitcher)
        {
            await _userPatternRepository.SaveAsync(new UserPatternRecord
            {
                User = user,
                Pattern = pattern,
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);
        }

        return Result.Success();
    }

    public async Task<Result<VerifyPatternResponse>> VerifyPattern(string patternData, CancellationToken cancellationToken)
    {
        var parseResult = _patternParserService.Parse(new ParsePatternParameters
        {
            RawContent = patternData
        });
        if (parseResult.IsFailure)
            return Result<VerifyPatternResponse>.FromFailure(parseResult);

        return new VerifyPatternResponse();
    }

    public async Task<Result<DeletePatternResponse>> DeletePattern(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var creatorResult = await _creatorRepository.GetByUser(user, cancellationToken);
        if (!creatorResult.TrySuccess(out var creator))
            return Result<DeletePatternResponse>.FromFailure(creatorResult);

        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<DeletePatternResponse>.FromFailure(patternResult);

        if (pattern.Creator.Id != creator.Id)
            return Result<DeletePatternResponse>.Failure("Unable to delete pattern as you are not a creator of it.");

        var doesProjectExist = await _userPatternRepository.DoesProjectExistForPatternAsync(pattern, cancellationToken);
        if (doesProjectExist)
        {
            // Mark as deleted in the record

            return new DeletePatternResponse
            {
                Message = "At least 1 user had this pattern, so it has been marked as deleted. It still exists, but won't show up for new users."
            };
        }

        var patternWithThreads = (await _patternRepository.GetWithThreadsByReferenceAsync(patternReference, cancellationToken)).Content;

        var stitches = (await _patternRepository.GetStitchesByThreads(patternWithThreads.Threads.ToList(), cancellationToken)).SelectMany(x => x.Value).ToList();

        await _patternThreadStitchRepository.DeleteManyAsync(stitches, cancellationToken);
        await _patternThreadRepository.DeleteManyAsync(patternWithThreads.Threads, cancellationToken);
        await _patternRepository.DeleteAsync(patternWithThreads, cancellationToken);

        return new DeletePatternResponse
        {
            Message = "No users had this pattern, so it has been permanently deleted."
        };
    }
}