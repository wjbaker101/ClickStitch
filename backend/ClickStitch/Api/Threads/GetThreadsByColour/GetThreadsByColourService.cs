using ClickStitch.Api.Threads.GetThreadsByColour.Types;
using Data.Repositories.Thread;
using Data.Repositories.Thread.Types;

namespace ClickStitch.Api.Threads.GetThreadsByColour;

public interface IGetThreadsByColourService
{
    Task<Result<GetThreadsByColourResponse>> GetThreadsByColour(GetThreadsByColourRequest request, CancellationToken cancellationToken);
}

public sealed class GetThreadsByColourService : IGetThreadsByColourService
{
    private readonly IThreadRepository _threadRepository;

    public GetThreadsByColourService(IThreadRepository threadRepository)
    {
        _threadRepository = threadRepository;
    }

    public async Task<Result<GetThreadsByColourResponse>> GetThreadsByColour(GetThreadsByColourRequest request, CancellationToken cancellationToken)
    {
        var threads = await _threadRepository.Search(new SearchThreadsParameters
        {
            SearchTerm = null,
            Brand = "DMC"
        }, cancellationToken);

        var lookup = threads.ToDictionary(x =>
        {
            var asInt = Convert.ToInt32(x.Colour.Replace("#", ""), 16);

            var r = (asInt & 0xff0000) >> 16;
            var g = (asInt & 0xff00) >> 8;
            var b = asInt & 0xff;

            return new Rgb(r, g, b);
        });

        var final = request.Colours.ConvertAll(x =>
        {
            var asRgb = new Rgb(x.R, x.G, x.B);

            var minDistance = double.MaxValue;
            var min = new Rgb(0, 0, 0);

            foreach (var (key, _) in lookup)
            {
                var distance = Distance(key, asRgb);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    min = key;
                }
            }

            return min;
        });

        return new GetThreadsByColourResponse
        {
            Threads = final.ConvertAll(x => ThreadMapper.Map(lookup[x]))
        };
    }

    private readonly record struct Rgb
    {
        public int R { get; }
        public int G { get; }
        public int B { get; }

        public Rgb(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }

    private static double Distance(Rgb first, Rgb second)
    {
        return Math.Sqrt(
            (first.R - second.R) * (first.R - second.R) +
            (first.G - second.G) * (first.G - second.G) +
            (first.B - second.B) * (first.B - second.B));
    }
}