using Core.Types;
using CrossStitchViewer.Api.Patterns.Types;
using CrossStitchViewer.Mappers;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.Pattern.Types;
using System.Text.Json;

namespace CrossStitchViewer.Api.Patterns;

public interface IPatternsService
{
    Result<GetPatternsResponse> GetPatterns();
    Result CreatePattern();
}

public sealed class PatternsService : IPatternsService
{
    private readonly IPatternRepository _patternRepository;
    private readonly IPatternStitchRepository _patternStitchRepository;
    private readonly IPatternThreadRepository _patternThreadRepository;

    public PatternsService(
        IPatternRepository patternRepository,
        IPatternStitchRepository patternStitchRepository,
        IPatternThreadRepository patternThreadRepository)
    {
        _patternRepository = patternRepository;
        _patternStitchRepository = patternStitchRepository;
        _patternThreadRepository = patternThreadRepository;
    }

    public Result<GetPatternsResponse> GetPatterns()
    {
        var patterns = _patternRepository.Search(new SearchPatternsParameters());

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
            Colour = x.colour
        }));

        return Result.Success();
    }
}