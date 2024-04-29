using ClickStitch.Api.Projects.CompleteBackStitches.Types;
using Data.Repositories.User;
using Data.Repositories.UserPatternThreadBackStitch;
using Data.Repositories.UserPatternThreadBackStitch.Types;

namespace ClickStitch.Api.Projects.CompleteBackStitches;

public interface ICompleteBackStitchesService
{
    Task<Result<CompleteBackStitchesResponse>> CompleteBackStitches(RequestUser requestUser, Guid patternReference, CompleteBackStitchesRequest request, CancellationToken cancellationToken);
}

public sealed class CompleteBackStitchesService : ICompleteBackStitchesService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternThreadBackStitchRepository _userPatternThreadBackStitchRepository;

    public CompleteBackStitchesService(IUserRepository userRepository, IUserPatternThreadBackStitchRepository userPatternThreadBackStitchRepository)
    {
        _userRepository = userRepository;
        _userPatternThreadBackStitchRepository = userPatternThreadBackStitchRepository;
    }

    public async Task<Result<CompleteBackStitchesResponse>> CompleteBackStitches(RequestUser requestUser, Guid patternReference, CompleteBackStitchesRequest request, CancellationToken cancellationToken)
    {
        if (request.BackStitchesByThread.Sum(x => x.Value.Count) > ProjectsHelper.MAX_COMPLETED_STITCHES_SELECTION_SIZE)
            return Result<CompleteBackStitchesResponse>.Failure($"The number of stitches to complete exceeds maximum ({ProjectsHelper.MAX_COMPLETED_STITCHES_SELECTION_SIZE}), please try again with a smaller selection.");

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        await _userPatternThreadBackStitchRepository.Complete(user, patternReference, new BackStitchPositions
        {
            StitchesByThread = request.BackStitchesByThread.ToDictionary(x => x.Key, x => x.Value.ConvertAll(y => new BackStitchPositions.Position
            {
                StartX = y.StartX,
                StartY = y.StartY,
                EndX = y.EndX,
                EndY = y.EndY
            }))
        }, cancellationToken);

        return new CompleteBackStitchesResponse();
    }
}