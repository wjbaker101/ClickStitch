using Core.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Types;

public abstract class ApiController : ControllerBase
{
    protected IActionResult ToApiResponse(Result result)
    {
        if (result.IsFailure)
        {
            return BadRequest(new ApiErrorResponse
            {
                FailureMessage = result.FailureMessage
            });
        }

        return Ok(new ApiResultResponse<bool>
        {
            Result = true
        });
    }

    protected IActionResult ToApiResponse<T>(Result<T> result)
    {
        if (result.IsFailure)
        {
            return BadRequest(new ApiErrorResponse
            {
                FailureMessage = result.FailureMessage
            });
        }

        return Ok(new ApiResultResponse<T>
        {
            Result = result.Content
        });
    }
}