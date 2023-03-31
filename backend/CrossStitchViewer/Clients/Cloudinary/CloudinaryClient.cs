using CloudinaryDotNet;
using Core.Settings;

namespace CrossStitchViewer.Clients.Cloudinary;

public interface ICloudinaryClient
{
}

public sealed class CloudinaryClient : ICloudinaryClient
{
    private CloudinaryDotNet.Cloudinary _client;

    public CloudinaryClient(AppSecrets secrets)
    {
        var cloudinary = secrets.Cloudinary;

        _client = new CloudinaryDotNet.Cloudinary(new Account(cloudinary.CloudName, cloudinary.ApiKey, cloudinary.ApiSecret));
    }
}