using MediatR;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Handlers;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Auth.Command.RefreshToken
{
    public class RefreshTokenCommandHandler(
        IIdentityService identity,
        IJwtService jwtService,
        IRefreshTokenRepository refreshToken,
        IUnitOfWork unitOfWork)
         : AuthCommandHandlerBase(identity, jwtService, refreshToken, unitOfWork),
            IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
    {
        private readonly IRefreshTokenRepository _refreshToken = refreshToken;
        public async Task<Result<AuthResponse>> Handle(
            RefreshTokenCommand request,
            CancellationToken ct = default)
        {
            var getUserToken = await _refreshToken.GetByTokenAsync(request.RefreshToken);
            if (getUserToken == null)
                return Result<AuthResponse>.Failure(Error.BadRequest("Token cannot be null"));
            if (!getUserToken.IsValid())
                return Result<AuthResponse>.Failure(Error.BadRequest("Token not valid"));

            await _refreshToken.RemoveExpiredOrRevokedAsync();
            getUserToken.Revoke();

            var token = await GenerateNewPairToken(getUserToken.UserId, ct);

            return Result<AuthResponse>.Success(token);
        }
    }
}
