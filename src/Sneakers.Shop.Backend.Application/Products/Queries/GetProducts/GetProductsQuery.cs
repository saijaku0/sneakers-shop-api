using MediatR;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Products.Queries.GetProducts
{
    public record GetProductsQuery(int Page, int PageSize) : IRequest<Result<GetProductsResponse>>;
}
