using ClickStitch.Api.Projects.CompleteBackStitches.Types;
using ClickStitch.Api.Projects.CompleteStitches.Types;
using Data.Repositories.User;
using Data.Repositories.UserPatternThreadBackStitch;
using Data.Repositories.UserPatternThreadBackStitch.Types;

namespace ClickStitch.Api.Projects.UnCompleteBackStitches;

public interface IUnCompleteBackStitchesService
{
    Task<Result<CompleteStitchesResponse>> UnCompleteBackStitches(RequestUser requestUser, Guid patternReference, CompleteBackStitchesRequest request, CancellationToken cancellationToken);
}

public sealed class UnCompleteBackStitchesService : IUnCompleteBackStitchesService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternThreadBackStitchRepository _userPatternThreadBackStitchRepository;

    public UnCompleteBackStitchesService(IUserRepository userRepository, IUserPatternThreadBackStitchRepository userPatternThreadBackStitchRepository)
    {
        _userRepository = userRepository;
        _userPatternThreadBackStitchRepository = userPatternThreadBackStitchRepository;
    }

    public async Task<Result<CompleteStitchesResponse>> UnCompleteBackStitches(RequestUser requestUser, Guid patternReference, CompleteBackStitchesRequest request, CancellationToken cancellationToken)
    {
        if (request.BackStitchesByThread.Sum(x => x.Value.Count) > ProjectsHelper.MAX_COMPLETED_STITCHES_SELECTION_SIZE)
            return Result<CompleteStitchesResponse>.Failure($"The number of stitches to un-complete exceeds maximum ({ProjectsHelper.MAX_COMPLETED_STITCHES_SELECTION_SIZE}), please try again with a smaller selection.");

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        await _userPatternThreadBackStitchRepository.UnComplete(user, patternReference, new BackStitchPositions
        {
            StitchesByThread = request.BackStitchesByThread.ToDictionary(x => x.Key, x => x.Value.ConvertAll(pos => new BackStitchPositions.Position
            {
                StartX = pos.StartX,
                StartY = pos.StartY,
                EndX = pos.EndX,
                EndY = pos.EndY
            }))
        }, cancellationToken);

        return new CompleteStitchesResponse();
    }
}