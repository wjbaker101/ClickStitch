﻿using ClickStitch.Api.Patterns.Types;
using ClickStitch.Clients.Cloudinary;
using ClickStitch.Clients.Cloudinary.Types;
using ClickStitch.Models.Mappers;
using Core.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.Pattern.Types;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using System.Text.Json;

namespace ClickStitch.Api.Patterns;

public interface IPatternsService
{
    Task<Result<GetPatternsResponse>> GetPatterns(RequestUser requestUser);
    Task<Result> CreatePattern();
    Task<Result> UpdatePatternImage(Guid patternReference, UpdatePatternImageRequest request);
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

    public async Task<Result<GetPatternsResponse>> GetPatterns(RequestUser requestUser)
    {
        var user = await _userRepository.GetByRequestUser(requestUser);

        var projects = await _userPatternRepository.GetByUserAsync(user);

        var patterns = await _patternRepository.SearchAsync(new SearchPatternsParameters
        {
            PatternFilter = projects.Select(x => x.Pattern).ToList()
        });

        return new GetPatternsResponse
        {
            Patterns = patterns.ConvertAll(PatternMapper.Map)
        };
    }

    public async Task<Result> CreatePattern()
    {
        var data = PatternData.GET;

        var json = JsonSerializer.Deserialize<PatternDataDto>(data)!;

        var pattern = await _patternRepository.SaveAsync(new PatternRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Title = "Bat Silhouette",
            Width = json.canvas.width,
            Height = json.canvas.height,
            Price = 2.5m,
            ThumbnailUrl = "",
            ThreadCount = json.palette.threads.Count - 1,
            StitchCount = json.canvas.stitches.Count,
            AidaCount = 14,
            Stitches = new HashSet<PatternStitchRecord>(),
            Threads = new HashSet<PatternThreadRecord>()
        });

        await _patternStitchRepository.SaveStitches(json.canvas.stitches.ConvertAll(x => new PatternStitchRecord
        {
            Pattern = pattern,
            ThreadIndex = x.index,
            X = x.x,
            Y = x.y
        }));

        await _patternThreadRepository.SaveManyAsync(json.palette.threads.ConvertAll(x => new PatternThreadRecord
        {
            Pattern = pattern,
            Name = x.name,
            Description = x.description,
            Index = x.index,
            Colour = $"#{x.colour.ToLower()}"
        }));

        return Result.Success();
    }

    public async Task<Result> UpdatePatternImage(Guid patternReference, UpdatePatternImageRequest request)
    {
        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference);
        if (!patternResult.TrySuccess(out var pattern))
            return Result.FromFailure(patternResult);

        var uploadResult = await _cloudinary.UploadImageAsync(new UploadImageRequest
        {
            FileName = request.FileName,
            FileContents = request.File.OpenReadStream()
        });
        if (!uploadResult.TrySuccess(out var upload))
            return Result.FromFailure(uploadResult);

        pattern.ThumbnailUrl = upload.Url;

        await _patternRepository.UpdateAsync(pattern);

        return Result.Success();
    }
}