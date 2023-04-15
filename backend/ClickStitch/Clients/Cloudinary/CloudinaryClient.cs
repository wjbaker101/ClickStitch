using ClickStitch.Clients.Cloudinary.Types;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.Settings;
using Core.Types;

namespace ClickStitch.Clients.Cloudinary;

public interface ICloudinaryClient
{
    Task<Result<UploadImageResponse>> UploadImageAsync(UploadImageRequest request, CancellationToken cancellationToken);
}

public sealed class CloudinaryClient : ICloudinaryClient
{
    private readonly CloudinaryDotNet.Cloudinary _client;

    public CloudinaryClient(AppSecrets secrets)
    {
        var cloudinary = secrets.Cloudinary;

        _client = new CloudinaryDotNet.Cloudinary(new Account(cloudinary.CloudName, cloudinary.ApiKey, cloudinary.ApiSecret));
    }

    public async Task<Result<UploadImageResponse>> UploadImageAsync(UploadImageRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.UploadAsync(new ImageUploadParams
        {
            File = new FileDescription(request.FileName, request.FileContents),
            PublicId = request.FileName
        }, cancellationToken);

        return new UploadImageResponse
        {
            Url = response.SecureUrl.ToString()
        };
    }
}