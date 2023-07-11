using ClickStitch.Api.Patterns.Parsing.Types;
using ClickStitch.Api.Patterns.Types;
using Core.Extensions;
using Data.Records;
using Utf8Json;

namespace ClickStitch.Api.Patterns.Parsing;

public interface IPatternParserService
{
    Result<PatternParseDetails> Parse(PatternParseParameters parameters);
}

public sealed class PatternParserService : IPatternParserService
{
    public PatternParserService()
    {
    }

    public Result<PatternParseDetails> Parse(PatternParseParameters parameters)
    {
        var data = JsonSerializer.Deserialize<CreatePatternData>(parameters.RawContent);

        var pattern = new PatternRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Title = parameters.Title,
            Width = data.canvas.width,
            Height = data.canvas.height,
            Price = parameters.Price,
            ThumbnailUrl = parameters.ThumbnailUrl,
            ThreadCount = data.palette.threads.Count - 1,
            StitchCount = data.canvas.stitches.Count,
            AidaCount = parameters.AidaCount,
            BannerImageUrl = parameters.BannerImageUrl,
            ExternalShopUrl = parameters.ExternalShopUrl,
            Creator = parameters.Creator,
            TitleSlug = parameters.TitleSlug,
            Threads = new HashSet<PatternThreadRecord>()
        };

        static bool IgnoreCanvasThread(CreatePatternData.Thread x) => x.index != 0;

        var threads = data.palette.threads
            .Where(IgnoreCanvasThread)
            .MapAll(x => new PatternThreadRecord
            {
                Pattern = pattern,
                Name = x.name,
                Description = x.description,
                Index = x.index,
                Colour = $"#{x.colour.ToLower()}"
            });

        var stitchesByThread = data.canvas.stitches
            .GroupBy(x => x.index)
            .ToDictionary(x => x.Key, x => x.ToList());

        var stitchRecords = new List<PatternThreadStitchRecord>();
        foreach (var thread in threads)
        {
            var stitches = stitchesByThread[thread.Index];

            stitchRecords.AddRange(stitches.ConvertAll(x => new PatternThreadStitchRecord
            {
                Thread = thread,
                X = x.x,
                Y = x.y,
                LookupHash = $"{x.x},{x.y}"
            }));
        }

        return new PatternParseDetails
        {
            Pattern = pattern,
            Threads = threads,
            Stitches = stitchRecords
        };
    }
}