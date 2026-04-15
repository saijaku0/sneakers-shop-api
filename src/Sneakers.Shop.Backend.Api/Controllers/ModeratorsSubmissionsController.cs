using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Shop.Backend.Api.Extensions;
using Sneakers.Shop.Backend.Api.Validators;
using Sneakers.Shop.Backend.Application.Submissions.Commands.ApproveSubmission;
using Sneakers.Shop.Backend.Application.Submissions.Commands.RejectSubmission;
using Sneakers.Shop.Backend.Application.Submissions.Commands.UpdateSubmissionDetails;
using Sneakers.Shop.Backend.Application.Submissions.Queries.GetPendingSubmissions;
using Sneakers.Shop.Backend.Application.Submissions.Queries.GetSubmissionById;
using System.Security.Claims;

namespace Sneakers.Shop.Backend.Api.Controllers
{
    /// <summary>
    /// Provides an API for managing moderation requests, including viewing, approving, rejecting, and updating
    /// requests available to the current moderator.
    /// </summary>
    /// <remarks>
    /// All controller actions require user authentication and that the user has active moderator permissions.
    /// The methods implement standard workflows for handling requests: retrieving a list, viewing details,
    /// approving, rejecting, and updating. Authorization policies are used for access control. Pagination
    /// and asynchronous request processing are supported.
    /// </remarks>
    /// <param name="mediator">
    /// An instance of the mediator used to send commands and queries to the application layer. Cannot be null.
    /// </param>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ModeratorsSubmissionsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

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

        /// <summary>
        /// Approves a submission with the specified identifier and attaches the provided files.
        /// </summary>
        /// <remarks>Requires the caller to be an active moderator as defined by the 'ActiveModerator'
        /// policy. Returns 401 if the user is not authenticated, 403 if the user lacks sufficient permissions, 404 if
        /// the submission does not exist, and 400 for invalid requests.</remarks>
        /// <param name="id">The unique identifier of the submission to approve.</param>
        /// <param name="files">A collection of files to associate with the approved submission. May be empty if no files are provided.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A result indicating the outcome of the operation. Returns 204 No Content if the approval is successful;
        /// returns appropriate error responses for invalid input, authorization failures, or if the submission is not
        /// found.</returns>
        [HttpPost("{id}/approve")]
        [Authorize(Policy = "ActiveModerator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApproveSubmission(
            [FromRoute] Guid id,
            [FromForm] IFormFileCollection files,
            CancellationToken ct)
        {
            var moderatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(moderatorId, out var parsedModeratorId))
                return Unauthorized();

            var (isValid, error) = FileValidator.Validate(files);
            if (!isValid)
                return BadRequest(error);

            var fileData = files.Select(f => (
                Stream: f.OpenReadStream(),
                f.FileName
            ));

            var command = new ApproveSubmissionCommand(
                SubmissionId: id,
                ModeratorId: parsedModeratorId,
                Files: fileData
            );

            var result = await _mediator.Send(command, ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Retrieves the details of a specific submission by its unique identifier.
        /// </summary>
        /// <remarks>Requires the caller to be an active moderator. Returns status code 200 if the
        /// submission is found, 404 if not found, 401 if the user is unauthorized, 403 if access is forbidden, or 500
        /// for an internal server error.</remarks>
        /// <param name="id">The unique identifier of the submission to retrieve.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> containing the submission details if found; otherwise, a result indicating
        /// the appropriate error status.</returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "ActiveModerator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSubmissionById(
            [FromRoute] Guid id,
            CancellationToken ct)
        {
            var moderatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(moderatorId, out var parsedModeratorId))
                return Unauthorized();
            var query = new GetSubmissionByIdQuery(null, id);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Updates the details of an existing submission with the specified identifier.
        /// </summary>
        /// <remarks>Requires the caller to be an authorized moderator with the 'ActiveModerator' policy.
        /// Returns 404 Not Found if the submission does not exist, 401 Unauthorized if the user is not authenticated,
        /// 403 Forbidden if the user lacks sufficient permissions, 400 Bad Request for invalid input, and 422
        /// Unprocessable Entity for validation errors.</remarks>
        /// <param name="id">The unique identifier of the submission to update.</param>
        /// <param name="request">An object containing the updated submission details. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A result indicating the outcome of the update operation. Returns 204 No Content if the update is successful;
        /// otherwise, returns an appropriate error response.</returns>
        [HttpPatch("{id}")]
        [Authorize(Policy = "ActiveModerator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSubmissionDetails(
            [FromRoute] Guid id,
            [FromBody] UpdateSubmissionDetailsCommand request,
            CancellationToken ct)
        {
            var moderatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(moderatorId, out var parsedModeratorId))
                return Unauthorized();
            var command = new UpdateSubmissionDetailsCommand(
                SubmissionId: id,
                ProductName: request.ProductName,
                Description: request.Description,
                TargetAudience: request.TargetAudience,
                Model: request.Model,
                BasePrice: request.BasePrice,
                BrandId: request.BrandId
            );
            var result = await _mediator.Send(command, ct);
            return result.ToActionResult(this);
        }
    }
}
