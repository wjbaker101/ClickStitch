using ClickStitch.Clients.Cloudinary;
using ClickStitch.Clients.Cloudinary.Types;
using DotNetLibs.Core.Types;

namespace TestHelpers.Fakes;

public sealed class FakeCloudinaryClient : ICloudinaryClient
{
    public Result<UploadImageResponse> Response { get; set; } = new UploadImageResponse
    {
        Url = "TestImageUrl"
    };

    public UploadImageRequest ActualRequest { get; private set; } = null!;

    public Task<Result<UploadImageResponse>> UploadImageAsync(UploadImageRequest request, CancellationToken cancellationToken)
    {
        ActualRequest = request;

        return Task.FromResult(Response);
    }
}