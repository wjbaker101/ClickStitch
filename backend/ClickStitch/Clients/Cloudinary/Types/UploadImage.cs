namespace ClickStitch.Clients.Cloudinary.Types;

public sealed class UploadImageRequest
{
    public required string FileName { get; init; }
    public required Stream FileContents { get; init; }
}

public sealed class UploadImageResponse
{
    public required string Url { get; init; }
}