using ClickStitch.Clients.Cloudinary.Types;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.Settings;

namespace ClickStitch.Clients.Cloudinary;

public interface ICloudinaryClient
{
    Task<Result<UploadImageResponse>> UploadImageAsync(UploadImageRequest request, CancellationToken cancellationToken);
}

public sealed class CloudinaryClient : ICloudinaryClient
{
    private readonly AppSettings _settings;
    private readonly CloudinaryDotNet.Cloudinary _client;

    public CloudinaryClient(AppSecrets secrets, AppSettings settings)
    {
        _settings = settings;

        _client = new CloudinaryDotNet.Cloudinary(new Account(secrets.Cloudinary.CloudName, secrets.Cloudinary.ApiKey, secrets.Cloudinary.ApiSecret));
    }

    public async Task<Result<UploadImageResponse>> UploadImageAsync(UploadImageRequest request, CancellationToken cancellationToken)
    {
        var response = await _client.UploadAsync(new ImageUploadParams
        {
            File = new FileDescription(request.FileName, request.FileContents),
            PublicId = $"{_settings.Environment}/{request.FileName}"
        }, cancellationToken);

        return new UploadImageResponse
        {
            Url = response.SecureUrl.ToString()
        };
    }
}