using Microsoft.AspNetCore.Http;

namespace TestHelpers.Fakes;

public sealed class FakeFormFile : IFormFile
{
    public byte[] Data { get; set; } = { 1, 2, 3 };

    public string ContentType => throw new NotImplementedException();
    public string ContentDisposition => throw new NotImplementedException();
    public IHeaderDictionary Headers => throw new NotImplementedException();
    public long Length => throw new NotImplementedException();
    public string Name => throw new NotImplementedException();
    public string FileName => throw new NotImplementedException();

    public Stream OpenReadStream()
    {
        return new MemoryStream(Data);
    }

    public void CopyTo(Stream target) => throw new NotImplementedException();
    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = new()) => throw new NotImplementedException();
}

public static class StreamExtensions
{
    public static byte[] ToBytes(this Stream stream)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        return memoryStream.ToArray();
    }
}