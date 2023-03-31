using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.Settings;
using Core.Types;
using CrossStitchViewer.Clients.Cloudinary.Types;

namespace CrossStitchViewer.Clients.Cloudinary;

public interface ICloudinaryClient
{
    Result<UploadImageResponse> UploadImage(UploadImageRequest request);
}

public sealed class CloudinaryClient : ICloudinaryClient
{
    private CloudinaryDotNet.Cloudinary _client;

    public CloudinaryClient(AppSecrets secrets)
    {
        var cloudinary = secrets.Cloudinary;

        _client = new CloudinaryDotNet.Cloudinary(new Account(cloudinary.CloudName, cloudinary.ApiKey, cloudinary.ApiSecret));
    }

    public Result<UploadImageResponse> UploadImage(UploadImageRequest request)
    {
        var response = _client.Upload(new ImageUploadParams
        {
            File = new FileDescription(request.FileName, request.FileContents),
            PublicId = request.FileName
        });

        return new UploadImageResponse
        {
            Url = response.SecureUrl.ToString()
        };
    }
}