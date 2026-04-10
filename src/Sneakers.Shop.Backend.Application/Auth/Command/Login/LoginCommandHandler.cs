using MediatR;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Handlers;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Auth.Command.Login
{
    public class LoginCommandHandler(
        IIdentityService identity,
        IJwtService jwtService,
        IRefreshTokenRepository refreshToken,
        IUnitOfWork unitOfWork) : AuthCommandHandlerBase(identity, jwtService, refreshToken, unitOfWork),
            IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        private readonly IIdentityService _identity = identity;
        public async Task<Result<AuthResponse>> Handle(
            LoginCommand request,
            CancellationToken ct = default)
        {
            var userId = await _identity.FindUserByEmailAsync(request.Email, ct);
            if (userId == null)
                return Result<AuthResponse>.Failure(Error.NotFound($"User is not found. {request.Email}"));
            var password = await _identity.CheckUserPassword(userId.Value, request.Password, ct);
            if (!password)
                return Result<AuthResponse>.Failure(Error.BadRequest("Login or password invalid"));

            var token = await GenerateNewPairToken(userId.Value, ct);
            return Result<AuthResponse>.Success(token);
        }
    }
}
