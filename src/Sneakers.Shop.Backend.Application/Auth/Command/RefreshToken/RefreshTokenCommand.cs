using MediatR;
using Sneakers.Shop.Backend.Application.Auth.DTOs;

namespace Sneakers.Shop.Backend.Application.Auth.Command.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponse>;
}
