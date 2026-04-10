using MediatR;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Application.Auth.Handlers;
using Sneakers.Shop.Backend.Application.Auth.Interfaces;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Auth.Command.RegisterNewUser
{
    public class RegisterCommandHandler(
        IIdentityService identity,
        IJwtService jwtService,
        IRefreshTokenRepository refreshToken,
        IUnitOfWork unitOfWork) 
        : AuthCommandHandlerBase(identity, jwtService, refreshToken, unitOfWork), 
            IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IIdentityService _identity = identity;
        public async Task<AuthResponse> Handle(
            RegisterCommand request,
            CancellationToken ct = default)
        {
            var existingUser = await _identity.FindUserByEmailAsync(request.Email, ct);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exist.");

            var userId = await _identity.CreateUser(request, ct);
            await _identity.AssignRole(userId, UserRole.Customer, ct);

            return await GenerateNewPairToken(userId, ct);
        }
    }
}
