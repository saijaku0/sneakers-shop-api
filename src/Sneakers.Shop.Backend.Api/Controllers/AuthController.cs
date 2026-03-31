using Microsoft.AspNetCore.Mvc;
using Sneakers.Shop.Backend.Api.DTOs;
using Sneakers.Shop.Backend.Api.Filters;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;

namespace Sneakers.Shop.Backend.Api.Controllers
{
    /// <summary>
    /// Defines API endpoints for user authentication operations such as login.
    /// </summary>
    /// <remarks>This controller provides endpoints for handling authentication-related actions. It is
    /// intended to be used as part of an ASP.NET Core Web API and relies on dependency injection for the authentication
    /// service.</remarks>
    /// <param name="authService">The authentication service used to process authentication requests.</param>
    [ApiController]
    [ValidationFilter]
    [Route("api/v1/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Authenticates a user based on the provided login credentials.
        /// </summary>
        /// <param name="request">The login request containing user credentials. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the login operation.</param>
        /// <returns>An IActionResult containing the authentication result if successful; otherwise, an unauthorized response.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var result = await _authService.LoginAsync(request, ct);
            return Ok(result);
        }

        /// <summary>
        /// Registers a new user account using the provided registration details.
        /// </summary>
        /// <param name="request">The registration information for the new user. Must not be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the registration operation.</param>
        /// <returns>An IActionResult indicating the result of the registration attempt. Returns a success response with
        /// registration details if successful; otherwise, returns a bad request with an error message.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
        {
            var result = await _authService.RegisterAsync(request, ct);
            return Ok(result);
        }

        /// <summary>
        /// Exchanges a valid refresh token for a new access token and refresh token pair.
        /// </summary>
        /// <param name="req">The request containing the refresh token to be validated and exchanged.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An IActionResult containing the new access and refresh tokens if the refresh token is valid; otherwise, an
        /// error response.</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest req, CancellationToken ct)
        {
            var result = await _authService.RefreshTokenAsync(req.RefreshToken, ct);
            return Ok(result);
        }
    }
}
