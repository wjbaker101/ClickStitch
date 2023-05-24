namespace Core.Extensions;

public static class EnumerableExtensions
{
    public static List<TOutput> MapAll<TInput, TOutput>(this IEnumerable<TInput> input, Func<TInput, TOutput> mapper) => input.Select(mapper).ToList();

    public static List<TOutput> MapAll<TInput, TOutput>(this IEnumerable<TInput> input, Func<TInput, int, TOutput> mapper) => input.Select(mapper).ToList();
}