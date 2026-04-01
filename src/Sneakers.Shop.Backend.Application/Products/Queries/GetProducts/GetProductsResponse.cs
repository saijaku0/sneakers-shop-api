using Sneakers.Shop.Backend.Application.Products.DTOs;

namespace Sneakers.Shop.Backend.Application.Products.Queries.GetProducts
{
    public record GetProductsResponse(IReadOnlyList<ProductsDto> Products, int TotalCount);
}
