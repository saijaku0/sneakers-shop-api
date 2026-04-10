using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Auth.Handlers
{
    public class AuthCommandHandlerBase (
        IIdentityService identity,
        IJwtService jwtService,
        IRefreshTokenRepository refreshToken,
        IUnitOfWork unitOfWork)
    {
        private readonly IIdentityService _identity = identity;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IRefreshTokenRepository _refreshToken = refreshToken;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        protected async Task<AuthResponse> GenerateNewPairToken(Guid userId, CancellationToken ct)
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
