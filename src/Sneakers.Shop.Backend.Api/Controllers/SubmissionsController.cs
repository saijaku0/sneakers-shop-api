using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Shop.Backend.Api.Extensions;
using Sneakers.Shop.Backend.Application.Submissions.Commands.CancelSubmission;
using Sneakers.Shop.Backend.Application.Submissions.Commands.CreateSubmission;
using Sneakers.Shop.Backend.Application.Submissions.Commands.RejectSubmission;
using Sneakers.Shop.Backend.Application.Submissions.Commands.UpdateSubmission;
using Sneakers.Shop.Backend.Application.Submissions.DTOs;
using Sneakers.Shop.Backend.Application.Submissions.Queries.GetListSubmission;
using Sneakers.Shop.Backend.Application.Submissions.Queries.GetPendingSubmissions;
using Sneakers.Shop.Backend.Application.Submissions.Queries.GetSubmissionById;
using Sneakers.Shop.Backend.Domain.Common;
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
        /// <remarks>This action is accessible only to users in the Dropper role. Returns 401 if the user
        /// is not authorized, 400 for invalid input, 404 if the resource is not found, and 500 for server
        /// errors.</remarks>
        /// <param name="command">The command containing the details required to create the submission.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An IActionResult containing the identifier of the created submission if successful; otherwise, an
        /// appropriate error response.</returns>
        [HttpPost]
        [Authorize(Policy = "ActiveDropper")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSubmission(
            [FromBody] CreateSubmissionCommand command,
            CancellationToken ct)
        {
            var dropId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(dropId, out var parsedDropId))
                return Unauthorized();
            var result = await _mediator.Send(command with { DropId = parsedDropId }, ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Retrieves a paginated list of submissions created by the currently authenticated dropper user.
        /// </summary>
        /// <remarks>This endpoint is accessible only to users with the Dropper role. The results are
        /// paginated based on the specified page and pageSize parameters.</remarks>
        /// <param name="page">The zero-based index of the page of submissions to retrieve. Must be greater than or equal to 0.</param>
        /// <param name="pageSize">The maximum number of submissions to include in the page. Must be a positive integer.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paginated list of submissions for the current user with status
        /// code 200 (OK) if successful; 401 (Unauthorized) if the user is not authenticated as a dropper; 404 (Not
        /// Found) if no submissions are found; or 500 (Internal Server Error) if an unexpected error occurs.</returns>
        [HttpGet("my-submissions")]
        [Authorize(Policy = "ActiveDropper")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMySubmissions(
            [FromQuery] int page,
            [FromQuery] int pageSize,
            CancellationToken ct)
        {
            var dropId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(dropId, out var parsedDropId))
                return Unauthorized();

            var query = new GetMySubmissionsQuery(parsedDropId, page, pageSize);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Deletes the submission with the specified identifier for the currently authenticated user.
        /// </summary>
        /// <remarks>This action is restricted to users with the Dropper role. The user must be
        /// authenticated, and only their own submissions can be deleted.</remarks>
        /// <param name="id">The unique identifier of the submission to delete.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A 204 No Content response if the submission is successfully deleted; a 401 Unauthorized response if the user
        /// is not authenticated; a 403 Forbidden response if the user does not have permission; a 404 Not Found
        /// response if the submission does not exist; or an appropriate error response for other failure conditions.</returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "ActiveDropper")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteSubmission(
            [FromRoute] Guid id,
            CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();
            var command = new CancelSubmissionCommand(id, parsedUserId);
            var result = await _mediator.Send(command, ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Updates an existing submission with the specified data.
        /// </summary>
        /// <remarks>Requires the caller to be authorized with the Dropper role. Returns 404 Not Found if
        /// the submission does not exist, 400 Bad Request for invalid input, 401 Unauthorized if the user is not
        /// authenticated, 403 Forbidden if the user does not have permission, and 422 Unprocessable Entity for
        /// validation errors.</remarks>
        /// <param name="id">The unique identifier of the submission to update.</param>
        /// <param name="request">The request object containing the updated submission data. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A result indicating the outcome of the update operation. Returns 204 No Content if the update is successful;
        /// otherwise, returns an appropriate error response.</returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "ActiveDropper")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSubmission(
            [FromRoute] Guid id,
            [FromBody] UpdateSubmissionRequest request,
            CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var command = new UpdateSubmissionCommand(
                DropId: parsedUserId,
                SubmissionId: id,
                request
            );

            var res = await _mediator.Send(command, ct);
            return res.ToActionResult(this);
        }

        /// <summary>
        /// Returns data for the current user's submission, identified as a Dropper, by the specified submission ID.
        /// </summary>
        /// <remarks>
        /// Access to this method is restricted to users with the Dropper role.
        /// Returns 401 (Unauthorized) if the user is not authenticated,
        /// and 403 (Forbidden) if the user does not have the required permissions.
        /// </remarks>
        /// <param name="submissionId">The unique identifier of the submission to retrieve.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>
        /// An IActionResult containing the submission data with status code 200 (OK) if found;
        /// 404 (NotFound) if the submission is not found;
        /// or an appropriate error status code in other cases.
        /// </returns>
        [HttpGet("my-submission")]
        [Authorize(Policy = "ActiveDropper")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> GetMySubmission(
            [FromQuery] Guid submissionId,
            CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();
            var query = new GetSubmissionByIdQuery(parsedUserId, submissionId);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Returns a list of pending applications available to the current moderator, with support
        /// for pagination.
        /// </summary>
        /// <remarks>
        /// Access to this method is allowed only for active moderators. Use query parameters
        /// to control pagination.
        /// </remarks>
        /// <param name="page">
        /// The page number of the results. Must be greater than or equal to 1. Defaults to 1.
        /// </param>
        /// <param name="pageSize">
        /// The number of applications per page. Must be greater than 0. Defaults to 10.
        /// </param>
        /// <param name="ct">
        /// A cancellation token that can be used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// An IActionResult object containing a page of pending applications. Returns status 200 (OK) with
        /// results, 401 (Unauthorized) if the user is not authenticated, 403 (Forbidden) if the user
        /// does not have moderator permissions, or 500 (Internal Server Error) in case of a server error.
        /// </returns>
        [HttpGet("pending")]
        [Authorize(Policy = "ActiveModerator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPendingSubmissions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var moderatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(moderatorId, out var parsedModeratorId))
                return Unauthorized();

            var query = new GetPendingSubmissionsQuery(parsedModeratorId, page, pageSize);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Rejects a pending submission with the specified reason as an authorized moderator.
        /// </summary>
        /// <remarks>Requires the caller to be an authenticated moderator with the 'ActiveModerator'
        /// policy. The moderator's identifier is determined from the current user context.</remarks>
        /// <param name="id">The unique identifier of the submission to reject.</param>
        /// <param name="request">The command containing the reason for rejection. Must not be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns 200 OK if the submission was
        /// successfully rejected; 400 Bad Request if the request is invalid; 401 Unauthorized if the user is not
        /// authenticated; 403 Forbidden if the user lacks permission; 404 Not Found if the submission does not exist;
        /// or 500 Internal Server Error for unexpected errors.</returns>
        [HttpPost("{id}/reject")]
        [Authorize(Policy = "ActiveModerator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RejectSubmission(
            [FromRoute] Guid id,
            [FromBody] RejectSubmissionCommand request,
            CancellationToken ct)
        {
            var moderatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(moderatorId, out var parsedModeratorId))
                return Unauthorized();
            var command = new RejectSubmissionCommand(
                SubmissionId: id,
                ModeratorId: parsedModeratorId,
                Reason: request.Reason
            );
            var result = await _mediator.Send(command, ct);
            return result.ToActionResult(this);
        }
    }
}
