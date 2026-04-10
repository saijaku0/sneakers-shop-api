using MediatR;
using Sneakers.Shop.Backend.Application.Auth.DTOs;

namespace Sneakers.Shop.Backend.Application.Auth.Command.Login
{
    public record LoginCommand(string Email, string Password) 
        : IRequest<AuthResponse>;
}
