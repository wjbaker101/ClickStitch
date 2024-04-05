using ClickStitch.Api.Projects.Types;
using Data.Repositories.User;
using Data.Repositories.UserPatternThreadStitch;
using Data.Repositories.UserPatternThreadStitch.Types;

namespace ClickStitch.Api.Projects.UnCompleteStitches;

public interface IUnCompleteStitchesService
{
    Task<Result<CompleteStitchesResponse>> UnCompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken);
}

public sealed class UnCompleteStitchesService : IUnCompleteStitchesService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternThreadStitchRepository _userPatternThreadStitchRepository;

    public UnCompleteStitchesService(IUserRepository userRepository, IUserPatternThreadStitchRepository userPatternThreadStitchRepository)
    {
        _userRepository = userRepository;
        _userPatternThreadStitchRepository = userPatternThreadStitchRepository;
    }

    public async Task<Result<CompleteStitchesResponse>> UnCompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken)
    {
        if (request.StitchesByThread.Sum(x => x.Value.Count) > ProjectsHelper.MAX_COMPLETED_STITCHES_SELECTION_SIZE)
            return Result<CompleteStitchesResponse>.Failure($"The number of stitches to un-complete exceeds maximum ({ProjectsHelper.MAX_COMPLETED_STITCHES_SELECTION_SIZE}), please try again with a smaller selection.");

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        await _userPatternThreadStitchRepository.UnComplete(user, patternReference, new StitchPosition
        {
            StitchesByThread = request.StitchesByThread.ToDictionary(x => x.Key, x => x.Value.ConvertAll(pos => new StitchPosition.Position
            {
                X = pos.X,
                Y = pos.Y
            }))
        }, cancellationToken);

        return new CompleteStitchesResponse();
    }
}