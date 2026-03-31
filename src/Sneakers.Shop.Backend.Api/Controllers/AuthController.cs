using Microsoft.AspNetCore.Mvc;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;
using Sneakers.Shop.Backend.Api.Filters;

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
        /// Attempts to refresh the authentication token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to use for obtaining a new authentication token. Cannot be null or empty.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the token refresh operation. Returns a successful
        /// result with the new token if the refresh is successful; otherwise, returns a bad request result with an
        /// error message.</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken, CancellationToken ct)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken, ct);
            return Ok(result);
        }
    }
}
