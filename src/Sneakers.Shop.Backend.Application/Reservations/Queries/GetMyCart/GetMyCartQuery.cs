using MediatR;
using Sneakers.Shop.Backend.Application.Reservations.Queries.Response;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Reservations.Queries.GetMyCart
{
    public record GetMyCartQuery(Guid UserId) : IRequest<Result<GetMyCartResponse>>;
}
