using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Shop.Backend.Api.Extensions;
using Sneakers.Shop.Backend.Application.Order.Command.CancelOrder;
using Sneakers.Shop.Backend.Application.Order.Command.UpdateOrderStatus;
using Sneakers.Shop.Backend.Application.Order.Queries.GetMyOrders;
using Sneakers.Shop.Backend.Application.Order.Queries.GetOrderById;
using Sneakers.Shop.Backend.Application.Orders.Command.CreateOrder;
using System.Security.Claims;

namespace Sneakers.Shop.Backend.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderCommand command,
            CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var result = await _mediator.Send(command with { UserId = parsedUserId }, ct);
            return result.ToActionResult(this);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyOrders(CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var query = new GetMyOrdersQuery(parsedUserId);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(
            [FromRoute] Guid id,
            CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var query = new GetOrderByIdQuery(id, parsedUserId);
            var result = await _mediator.Send(query, ct);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelOrder(
            [FromRoute] Guid id,
            CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var command = new CancelOrderCommand(id, parsedUserId);
            var result = await _mediator.Send(command, ct);
            return result.ToActionResult(this);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Policy = "ActiveModerator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] Guid id,
            [FromBody] UpdateOrderStatusCommand command,
            CancellationToken ct)
        {
            var result = await _mediator.Send(command with { OrderId = id }, ct);
            return result.ToActionResult(this);
        }
    }
}