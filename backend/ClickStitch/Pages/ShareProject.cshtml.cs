using Data.Repositories.Pattern;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternThreadStitch;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClickStitch.Pages;

public sealed class ShareProjectModel : PageModel
{
    public float PercentageCompleted { get; private set; }
    public string ImageUrl { get; private set; } = "";

    private readonly IUserPatternRepository _userPatternRepository;
    private readonly IPatternRepository _patternRepository;
    private readonly IUserPatternThreadStitchRepository _userPatternThreadStitchRepository;

    public ShareProjectModel(
        IUserPatternRepository userPatternRepository,
        IPatternRepository patternRepository,
        IUserPatternThreadStitchRepository userPatternThreadStitchRepository)
    {
        _userPatternRepository = userPatternRepository;
        _patternRepository = patternRepository;
        _userPatternThreadStitchRepository = userPatternThreadStitchRepository;
    }

    public async Task OnGetAsync(Guid projectReference, CancellationToken cancellationToken)
    {
        var projectResult = await _userPatternRepository.GetByReference(projectReference, cancellationToken);
        if (!projectResult.TrySuccess(out var project))
            return;

        var user = project.User;

        var patternResult = await _patternRepository.GetByReferenceAsync(project.Pattern.Reference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return;

        var completedStitches = await _userPatternThreadStitchRepository.GetByUser(user, pattern.Reference, cancellationToken);
        
        var maxStitches = pattern.StitchCount;
        var completedStitchCount = completedStitches.Values.SelectMany(x => x.Values).Count();

        var baseDomain = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

        PercentageCompleted = (float)completedStitchCount / maxStitches * 100;
        ImageUrl = $"{baseDomain}/share/projects/{project.Reference}";
    }
}