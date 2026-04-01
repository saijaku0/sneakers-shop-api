using MediatR;

namespace Sneakers.Shop.Backend.Application.Products.Queries.GetProducts
{
    public record GetProductsQuery(int Page, int PageSize) : IRequest<GetProductsResponse>;
}
