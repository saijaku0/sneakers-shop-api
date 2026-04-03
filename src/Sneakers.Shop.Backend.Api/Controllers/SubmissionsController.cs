using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Shop.Backend.Application.Submissions.Commands.CreateSubmission;
using Sneakers.Shop.Backend.Domain.Enums;
using System.Security.Claims;

namespace Sneakers.Shop.Backend.Api.Controllers
{
    /// <summary>
    /// Represents an API controller that manages submission-related operations for authenticated users.
    /// </summary>
    /// <remarks>This controller requires authentication for all actions. Endpoints are versioned under
    /// 'api/v1/submissions'. The controller relies on the mediator pattern to decouple request handling from business
    /// logic.</remarks>
    /// <param name="mediator">The mediator used to send commands and queries related to submissions. Cannot be null.</param>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubmissionsController(IMediator mediator) 
        : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Creates a new submission for the current dropper user.
        /// </summary>
        /// <remarks>This action is restricted to users with the Dropper role. The submission is
        /// associated with the authenticated user's drop identifier.</remarks>
        /// <param name="command">The command containing the details required to create the submission. Must not be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the operation. Returns 200 OK with the
        /// submission identifier if successful; otherwise, returns 401 Unauthorized if the user is not authenticated as
        /// a dropper.</returns>
        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Dropper))]
        public async Task<IActionResult> CreateSubmission(
            [FromBody] CreateSubmissionCommand command, 
            CancellationToken ct)
        {
            var dropId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(dropId, out var parsedDropId))
                return Unauthorized();
            var submissionId = await _mediator.Send(command with { DropId = parsedDropId }, ct);
            return Ok(submissionId);
        }
    }
}
