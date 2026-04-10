using MediatR;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Handlers;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Auth.Command.RefreshToken
{
    public class RefreshTokenCommandHandler(
        IIdentityService identity,
        IJwtService jwtService,
        IRefreshTokenRepository refreshToken,
        IUnitOfWork unitOfWork)
         : AuthCommandHandlerBase(identity, jwtService, refreshToken, unitOfWork),
            IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IRefreshTokenRepository _refreshToken = refreshToken;
        public async Task<AuthResponse> Handle(
            RefreshTokenCommand request,
            CancellationToken ct = default)
        {
            var getUserToken = await _refreshToken.GetByTokenAsync(request.RefreshToken)
                ?? throw new InvalidOperationException("Token cannot be null");
            if (!getUserToken.IsValid())
                throw new InvalidOperationException("Throw token not valid");

            await _refreshToken.RemoveExpiredOrRevokedAsync();
            getUserToken.Revoke();

            return await GenerateNewPairToken(getUserToken.UserId, ct);
        }
    }
}
