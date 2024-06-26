﻿using ClickStitch.Api.Projects.CompleteStitches.Types;
using Data.Repositories.User;
using Data.Repositories.UserPatternThreadStitch;
using Data.Repositories.UserPatternThreadStitch.Types;

namespace ClickStitch.Api.Projects.CompleteStitches;

public interface ICompleteStitchesService
{
    Task<Result<CompleteStitchesResponse>> CompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken);
}

public sealed class CompleteStitchesService : ICompleteStitchesService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternThreadStitchRepository _userPatternThreadStitchRepository;

    public CompleteStitchesService(IUserRepository userRepository, IUserPatternThreadStitchRepository userPatternThreadStitchRepository)
    {
        _userRepository = userRepository;
        _userPatternThreadStitchRepository = userPatternThreadStitchRepository;
    }

    public async Task<Result<CompleteStitchesResponse>> CompleteStitches(RequestUser requestUser, Guid patternReference, CompleteStitchesRequest request, CancellationToken cancellationToken)
    {
        if (request.StitchesByThread.Sum(x => x.Value.Count) > ProjectsHelper.MAX_COMPLETED_STITCHES_SELECTION_SIZE)
            return Result<CompleteStitchesResponse>.Failure($"The number of stitches to complete exceeds maximum ({ProjectsHelper.MAX_COMPLETED_STITCHES_SELECTION_SIZE}), please try again with a smaller selection.");

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        await _userPatternThreadStitchRepository.Complete(user, patternReference, new StitchPosition
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