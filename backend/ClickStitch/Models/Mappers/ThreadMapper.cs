using Data.Records;

namespace ClickStitch.Models.Mappers;

public static class ThreadMapper
{
    public static ThreadModel Map(ThreadRecord thread) => new()
    {
        Reference = thread.Reference,
        Brand = thread.Brand,
        Code = thread.Code,
        Colour = thread.Colour
    };
}