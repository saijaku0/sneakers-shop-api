using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;
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
