using MediatR;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Auth.Command.Login
{
    public record LoginCommand(string Email, string Password) 
        : IRequest<Result<AuthResponse>>;
}
