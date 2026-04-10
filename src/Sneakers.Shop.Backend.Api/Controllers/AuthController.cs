using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sneakers.Shop.Backend.Api.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Command.Login;
using Sneakers.Shop.Backend.Application.Auth.Command.RefreshToken;
using Sneakers.Shop.Backend.Application.Auth.Command.RegisterNewUser;

namespace Sneakers.Shop.Backend.Api.Controllers
{
    /// <summary>
    /// Defines API endpoints for user authentication operations, including login, registration, and token refresh
    /// functionality.
    /// </summary>
    /// <remarks>This controller provides endpoints for common authentication workflows in the application.
    /// All actions are accessible via the 'api/v1/auth' route prefix. Each endpoint expects the relevant request data
    /// in the request body and returns an appropriate HTTP response based on the operation outcome.</remarks>
    /// <param name="mediator">The mediator instance used to send authentication-related commands and queries. Cannot be null.</param>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Authenticates a user based on the provided login credentials.
        /// </summary>
        /// <param name="request">The login command containing the user's credentials. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the login operation.</param>
        /// <returns>An IActionResult containing the authentication result. Returns a success response with authentication
        /// details if the credentials are valid; otherwise, returns an error response.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand request, 
            CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return Ok(result);
        }

        /// <summary>
        /// Registers a new user account using the provided registration details.
        /// </summary>
        /// <remarks>This endpoint is typically used to create a new user in the system. The response
        /// includes the outcome of the registration process. If registration fails due to validation errors or other
        /// issues, an appropriate error response is returned.</remarks>
        /// <param name="request">The registration information required to create a new user account.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An IActionResult containing the result of the registration operation.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterCommand request, 
            CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return Ok(result);
        }

        /// <summary>
        /// Handles a request to refresh an access token using a valid refresh token.
        /// </summary>
        /// <remarks>This endpoint is typically used to obtain a new access token when the current one has
        /// expired, provided the refresh token is still valid. The client must supply a valid refresh token in the
        /// request body.</remarks>
        /// <param name="req">The request containing the refresh token to be validated and exchanged for a new access token. Cannot be
        /// null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An IActionResult containing the new access token and related information if the refresh token is valid;
        /// otherwise, an error response.</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequest req, 
            CancellationToken ct)
        {
            var result = await _mediator.Send(new RefreshTokenCommand(req.RefreshToken), ct);
            return Ok(result);
        }
    }
}
