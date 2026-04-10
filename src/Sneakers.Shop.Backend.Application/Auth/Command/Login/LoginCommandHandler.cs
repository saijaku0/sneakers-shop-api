using MediatR;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Handlers;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Auth.Command.Login
{
    public class LoginCommandHandler(
        IIdentityService identity,
        IJwtService jwtService,
        IRefreshTokenRepository refreshToken,
        IUnitOfWork unitOfWork) : AuthCommandHandlerBase(identity, jwtService, refreshToken, unitOfWork),
            IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IIdentityService _identity = identity;
        public async Task<AuthResponse> Handle(
            LoginCommand request,
            CancellationToken ct = default)
        {
            var userId = await _identity.FindUserByEmailAsync(request.Email, ct)
                ?? throw new InvalidOperationException($"User is not found. {request.Email}");
            var password = await _identity.CheckUserPassword(userId, request.Password, ct);
            if (!password)
                throw new InvalidOperationException("Login or password invalid");

            return await GenerateNewPairToken(userId, ct);
        }
    }
}
