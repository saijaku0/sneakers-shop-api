using MediatR;
using Sneakers.Shop.Backend.Application.Auth.DTOs;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.ValueObjects;

namespace Sneakers.Shop.Backend.Application.Auth.Command.RegisterNewUser
{
    public record RegisterCommand(
        string Name,
        string Lastname,
        string PhoneNumber,
        string Email,
        string Password,
        Address? DefaultShippingAddress) : IRequest<Result<AuthResponse>>;
}
