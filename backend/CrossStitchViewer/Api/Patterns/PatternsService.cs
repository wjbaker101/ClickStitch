using Core.Types;
using CrossStitchViewer.Api.Patterns.Types;
using CrossStitchViewer.Clients.Cloudinary;
using CrossStitchViewer.Clients.Cloudinary.Types;
using CrossStitchViewer.Mappers;
using CrossStitchViewer.Models;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.Pattern.Types;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using System.Text.Json;

namespace CrossStitchViewer.Api.Patterns;

public interface IPatternsService
{
    Result<GetPatternsResponse> GetPatterns(UserModel requestUser);
    Result CreatePattern();
    Result UpdatePatternImage(Guid patternReference, UpdatePatternImageRequest request);
}

public sealed class PatternsService : IPatternsService
{
    private readonly IPatternRepository _patternRepository;
    private readonly IPatternStitchRepository _patternStitchRepository;
    private readonly IPatternThreadRepository _patternThreadRepository;
    private readonly ICloudinaryClient _cloudinary;
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;

    public PatternsService(
        IPatternRepository patternRepository,
        IPatternStitchRepository patternStitchRepository,
        IPatternThreadRepository patternThreadRepository,
        ICloudinaryClient cloudinary,
        IUserRepository userRepository,
        IUserPatternRepository userPatternRepository)
    {
        _patternRepository = patternRepository;
        _patternStitchRepository = patternStitchRepository;
        _patternThreadRepository = patternThreadRepository;
        _cloudinary = cloudinary;
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
    }

    public Result<GetPatternsResponse> GetPatterns(UserModel requestUser)
    {
        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetPatternsResponse>.FromFailure(userResult);

        var projects = _userPatternRepository.GetByUser(user);

        var patterns = _patternRepository.Search(new SearchPatternsParameters
        {
            PatternFilter = projects.Select(x => x.Pattern).ToList()
        });

        return new GetPatternsResponse
        {
            Patterns = patterns.ConvertAll(PatternMapper.Map)
        };
    }

    public Result CreatePattern()
    {
        var data = PatternData.GET;

        var json = JsonSerializer.Deserialize<PatternDataDto>(data)!;

        var pattern = _patternRepository.Save(new PatternRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Title = "Templar Knight",
            Width = json.canvas.width,
            Height = json.canvas.height,
            Price = 2.5m,
            ThumbnailUrl = null,
            ThreadCount = json.palette.threads.Count,
            StitchCount = json.canvas.stitches.Count,
            AidaCount = 16,
            Stitches = new HashSet<PatternStitchRecord>(),
            Threads = new HashSet<PatternThreadRecord>()
        });

        _patternStitchRepository.SaveMany(json.canvas.stitches.ConvertAll(x => new PatternStitchRecord
        {
            Pattern = pattern,
            ThreadIndex = x.index,
            X = x.x,
            Y = x.y
        }));

        _patternThreadRepository.SaveMany(json.palette.threads.ConvertAll(x => new PatternThreadRecord
        {
            Pattern = pattern,
            Name = x.name,
            Description = x.description,
            Index = x.index,
            Colour = $"#{x.colour.ToLower()}"
        }));

        return Result.Success();
    }

    public Result UpdatePatternImage(Guid patternReference, UpdatePatternImageRequest request)
    {
        var patternResult = _patternRepository.GetByReference(patternReference);
        if (!patternResult.TrySuccess(out var pattern))
            return Result.FromFailure(patternResult);

        var uploadResult = _cloudinary.UploadImage(new UploadImageRequest
        {
            FileName = request.FileName,
            FileContents = request.File.OpenReadStream()
        });
        if (!uploadResult.TrySuccess(out var upload))
            return Result.FromFailure(uploadResult);

        pattern.ThumbnailUrl = upload.Url;

        _patternRepository.Update(pattern);

        return Result.Success();
    }
}