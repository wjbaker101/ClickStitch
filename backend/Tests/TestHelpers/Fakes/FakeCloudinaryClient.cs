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

    public string ActualFileName { get; private set; }
    public byte[] ActualFileContents { get; private set; }

    public Task<Result<UploadImageResponse>> UploadImageAsync(UploadImageRequest request, CancellationToken cancellationToken)
    {
        ActualFileName = request.FileName;
        ActualFileContents = request.FileContents.ToBytes();

        return Task.FromResult(Response);
    }
}