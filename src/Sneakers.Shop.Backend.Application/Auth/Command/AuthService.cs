using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;
using Sneakers.Shop.Backend.Application.Interfaces;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Application.Auth.Command
{
    public class AuthService(
        IIdentityService identity, 
        IJwtService jwtService, 
        IRefreshTokenRepository refreshToken,
        IUnitOfWork unitOfWork) : IAuthService
    {
        private readonly IIdentityService _identity = identity;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IRefreshTokenRepository _refreshToken = refreshToken;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Asynchronously registers a new user with the specified registration details.    
        /// </summary>
        /// <param name="request">The registration information for the new user. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the registration operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an AuthResponse with the outcome
        /// of the registration.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a user with the specified email address already exists.</exception>
        public async Task<AuthResponse> RegisterAsync(
            RegisterRequest request, 
            CancellationToken ct = default)
        {
            var existingUser = await _identity.FindUserByEmailAsync(request.Email, ct);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exist.");

            var userId = await _identity.CreateUser(request, ct);

            await _identity.AssignRole(userId, UserRole.Customer, ct);

            return await GenerateNewPairToken(userId, ct);
        }

        /// <summary>
        /// Authenticates a user asynchronously using the provided login credentials and returns an authentication
        /// response containing access tokens.
        /// </summary>
        /// <param name="request">The login request containing the user's email and password. Cannot be null.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the login operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an authentication response with
        /// access and refresh tokens for the authenticated user.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user is not found or if the login credentials are invalid.</exception>
        public async Task<AuthResponse> LoginAsync(
            LoginRequest request,
            CancellationToken ct = default)
        {
            var userId = await _identity.FindUserByEmailAsync(request.Email, ct)
                ?? throw new InvalidOperationException($"User is not found. {request.Email}");
            var password = await _identity.CheckUserPassword(userId, request.Password, ct);
            if (!password) 
                throw new InvalidOperationException("Login or password invalid");

            return await GenerateNewPairToken(userId, ct);
        }

        /// <summary>
        /// Generates a new authentication token pair using the specified refresh token.
        /// </summary>
        /// <remarks>The method revokes the provided refresh token and issues a new token pair. The old
        /// token cannot be reused after this operation.</remarks>
        /// <param name="refreshToken">The refresh token used to request a new authentication token pair. Cannot be null or invalid.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="AuthResponse"/>
        /// with the new authentication token pair.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the specified refresh token is null, not found, or is not valid.</exception>
        public async Task<AuthResponse> RefreshTokenAsync
            (string refreshToken, 
            CancellationToken ct = default)
        {
            var getUserToken = await _refreshToken.GetByTokenAsync(refreshToken)
                ?? throw new InvalidOperationException("Token cannot be null");
            if (!getUserToken.IsValid())
                throw new InvalidOperationException("Throw token not valid");

            await _refreshToken.RemoveExpiredOrRevokedAsync();
            getUserToken.Revoke();

            return await GenerateNewPairToken(getUserToken.UserId, ct);
        }

        /// <summary>
        /// Generates a new pair of access and refresh tokens for the specified user.
        /// </summary>
        /// <remarks>The generated refresh token is persisted and associated with the specified user.
        /// Existing refresh tokens are not revoked by this method.</remarks>
        /// <param name="userId">The unique identifier of the user for whom the tokens are generated.</param>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an AuthResponse with the newly
        /// generated access and refresh tokens.</returns>
        private async Task<AuthResponse> GenerateNewPairToken(Guid userId, CancellationToken ct)
        {
            var tokenData = await _identity.GetTokenGenerationDataAsync(userId, ct);
            var accessToken = _jwtService.GenerateAccessToken(tokenData);
            var updateToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(
                userId: userId,
                token: updateToken,
                expiresAt: DateTimeOffset.UtcNow.AddDays(_jwtService.GetRefreshTokenExpiryDays())
            );

            await _refreshToken.AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync(ct);

            return new AuthResponse(accessToken, updateToken);
        }
    }
}
