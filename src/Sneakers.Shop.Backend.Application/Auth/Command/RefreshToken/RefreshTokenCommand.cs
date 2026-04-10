using MediatR;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Auth.Command.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<AuthResponse>>;
}
