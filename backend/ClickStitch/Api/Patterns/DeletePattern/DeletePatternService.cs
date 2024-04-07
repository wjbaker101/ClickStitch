using ClickStitch.Api.Patterns.DeletePattern.Types;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace ClickStitch.Api.Patterns.DeletePattern;

public interface IDeletePatternService
{
    Task<Result<DeletePatternResponse>> DeletePattern(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
}

public sealed class DeletePatternService : IDeletePatternService
{
    private readonly IPatternRepository _patternRepository;
    private readonly IPatternThreadRepository _patternThreadRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;
    private readonly IPatternThreadStitchRepository _patternThreadStitchRepository;

    public DeletePatternService(
        IPatternRepository patternRepository,
        IPatternThreadRepository patternThreadRepository,
        IUserRepository userRepository,
        IUserPatternRepository userPatternRepository,
        IPatternThreadStitchRepository patternThreadStitchRepository)
    {
        _patternRepository = patternRepository;
        _patternThreadRepository = patternThreadRepository;
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
        _patternThreadStitchRepository = patternThreadStitchRepository;
    }

    public async Task<Result<DeletePatternResponse>> DeletePattern(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<DeletePatternResponse>.FromFailure(patternResult);

        if (pattern.User.Id != user.Id)
            return Result<DeletePatternResponse>.Failure("Unable to delete pattern as you are not a creator of it.");

        if (pattern.Creator != null)
        {
            var doesProjectExist = await _userPatternRepository.DoesProjectExistForPatternAsync(pattern, cancellationToken);
            if (doesProjectExist)
            {
                pattern.IsPublic = false;

                await _patternRepository.UpdateAsync(pattern, cancellationToken);

                return new DeletePatternResponse
                {
                    Message = "At least 1 user had this pattern, so it has been marked as deleted. It still exists, but won't show up for new users."
                };
            }
        }

        var patternWithThreads = (await _patternRepository.GetWithThreadsByReferenceAsync(patternReference, cancellationToken)).Content;

        await _patternThreadStitchRepository.DeleteByThreads(patternWithThreads.Threads, cancellationToken);
        await _patternThreadRepository.DeleteManyAsync(patternWithThreads.Threads, cancellationToken);
        await _patternRepository.DeleteAsync(patternWithThreads, cancellationToken);

        return new DeletePatternResponse
        {
            Message = "No users had this pattern, so it has been permanently deleted."
        };
    }
}