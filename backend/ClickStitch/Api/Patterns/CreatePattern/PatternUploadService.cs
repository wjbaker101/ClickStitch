using ClickStitch.Api.Patterns.CreatePattern.Types;
using ClickStitch.Clients.Cloudinary;
using ClickStitch.Clients.Cloudinary.Types;

namespace ClickStitch.Api.Patterns.CreatePattern;

public interface IPatternUploadService
{
    Task<Result<string>> UploadImage(string folderName, PatternImageType imageType, Stream contents, CancellationToken cancellationToken);
}

public sealed class PatternUploadService : IPatternUploadService
{
    private readonly ICloudinaryClient _cloudinary;

    public PatternUploadService(ICloudinaryClient cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<Result<string>> UploadImage(string folderName, PatternImageType imageType, Stream contents, CancellationToken cancellationToken)
    {
        var imageResult = await _cloudinary.UploadImageAsync(new UploadImageRequest
        {
            FileName = $"patterns/{folderName}/{MapImageType(imageType)}",
            FileContents = contents
        }, cancellationToken);

        if (imageResult.IsFailure)
            return Result<string>.FromFailure(imageResult);

        return imageResult.Content.Url;
    }

    private static string MapImageType(PatternImageType imageType) => imageType switch
    {
        PatternImageType.Thumbnail => "thumbnail",
        PatternImageType.Banner => "banner",

        PatternImageType.Unknown or _ => throw new NotSupportedException($"The given image type was unknown or invalid: '{imageType}'.")
    };
}