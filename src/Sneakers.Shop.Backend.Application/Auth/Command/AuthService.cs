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

            var tokenData = await _identity.GetTokenGenerationDataAsync(userId, ct);
            var accessToken = _jwtService.GenerateAccessToken(tokenData);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(
                userId: userId,
                token: refreshToken,
                expiresAt: DateTimeOffset.UtcNow.AddDays(_jwtService.GetRefreshTokenExpiryDays())
            );

            await _refreshToken.AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync(ct);

            return new AuthResponse(accessToken, refreshToken);
        }

        public Task<AuthResponse> LoginAsync(
            LoginRequest request,
            CancellationToken ct = default)
        {
            
        }

        public Task<AuthResponse> RefreshTokenAsync
            (string refreshToken, 
            CancellationToken ct = default)
        {

        }
    }
}
