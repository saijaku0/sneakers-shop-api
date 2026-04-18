using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Shop.Backend.Api.Extensions;
using Sneakers.Shop.Backend.Application.Reservations.Command.CancelReservation;
using Sneakers.Shop.Backend.Application.Reservations.Command.CreateReservation;
using Sneakers.Shop.Backend.Application.Reservations.Command.UpdateReservationQuantity;
using Sneakers.Shop.Backend.Application.Reservations.Queries.GetMyCart;
using System.Security.Claims;

namespace Sneakers.Shop.Backend.Api.Controllers
{
    /// <summary>
    /// API controller for managing the cart and creating reservations; requires authentication and uses IMediator
    /// to handle commands.
    /// </summary>
    /// <remarks>
    /// Controller route: api/v1/[controller]. The CreateReservation action handles POST requests,
    /// extracts the user ID from claims, and assigns it to the command before sending it through IMediator.
    /// </remarks>
    /// <param name="mediator">
    /// The IMediator mediator used to send commands and handle requests/responses within the controller.
    /// </param>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Creates a reservation for the authenticated user by sending a CreateReservationCommand via IMediator
        /// and returning the corresponding IActionResult.
        /// </summary>
        /// <remarks>
        /// Authentication is required. The user ID is extracted from ClaimTypes.NameIdentifier and parsed into a GUID;
        /// if parsing fails, a 401 response is returned. The command is sent via IMediator and the result is
        /// transformed into an IActionResult.
        /// </remarks>
        /// <param name="command">
        /// The command containing reservation data; before being sent via IMediator, the UserId of the current
        /// user (from claims) is assigned to it.
        /// </param>
        /// <param name="ct">
        /// Cancellation token for the asynchronous operation.
        /// </param>
        /// <returns>
        /// IActionResult representing the operation result: 200 for successful creation, 400 for invalid data,
        /// 401 for missing or invalid user identifier, 404 if required resources are not found.
        /// </returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateReservation(
            [FromBody] CreateReservationCommand command, 
            CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var result = await _mediator.Send(command with { UserId = parsedUserId }, ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Returns the cart of the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// The user ID is extracted from the ClaimsPrincipal; if the GUID cannot be parsed,
        /// a 401 response is returned.
        /// </remarks>
        /// <param name="ct">
        /// Cancellation token for the asynchronous operation.
        /// </param>
        /// <returns>
        /// IActionResult containing the user's cart on success (200), or 401 if the user identifier
        /// is missing or invalid.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyCart(CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var query = new GetMyCartQuery(parsedUserId);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Updates the quantity of a reservation with the specified identifier for the current user.
        /// </summary>
        /// <remarks>
        /// The user ID is extracted from ClaimTypes.NameIdentifier. The update is performed via IMediator
        /// by sending an UpdateReservationQuantityCommand.
        /// </remarks>
        /// <param name="id">
        /// The reservation identifier.
        /// </param>
        /// <param name="newQuantity">
        /// The new quantity for the reservation.
        /// </param>
        /// <param name="ct">
        /// Cancellation token for the operation.
        /// </param>
        /// <returns>
        /// IActionResult representing the operation result: 204 on successful update; 400 for invalid data;
        /// 401 for unauthenticated user; 404 if the reservation is not found.
        /// </returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateQuantity(
            [FromRoute] Guid id,
            [FromBody] int newQuantity,
            CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var command = new UpdateReservationQuantityCommand(id, parsedUserId, newQuantity);
            var result = await _mediator.Send(command, ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Cancels a reservation with the specified identifier on behalf of the authenticated user.
        /// </summary>
        /// <remarks>
        /// Requires authentication — the user ID is extracted from ClaimTypes.NameIdentifier.
        /// Executes CancelReservationCommand via IMediator.
        /// </remarks>
        /// <param name="id">
        /// The identifier of the reservation to cancel.
        /// </param>
        /// <param name="ct">
        /// Cancellation token for the asynchronous operation.
        /// </param>
        /// <returns>
        /// 204 NoContent on successful cancellation; 401 Unauthorized if the user identifier is missing or invalid;
        /// 404 NotFound if the reservation is not found.
        /// </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelReservation(
            [FromRoute] Guid id,
            CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var command = new CancelReservationCommand(id, parsedUserId);
            var result = await _mediator.Send(command, ct);
            return result.ToActionResult(this);
        }
    }
}
