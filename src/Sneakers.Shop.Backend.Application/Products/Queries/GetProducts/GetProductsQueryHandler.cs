using MediatR;
using Sneakers.Shop.Backend.Application.Products.DTOs;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler(IProductRepository productRepository)
        : IRequestHandler<GetProductsQuery, GetProductsResponse>
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<GetProductsResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var (page, pageSize) = request;
            var products = await _productRepository.GetAllActiveAsync(page, pageSize, cancellationToken);
            var totalCount = await _productRepository.GetTotalProductsCountAsync(cancellationToken);

            var productDtos = products.Select(p => new ProductsDto(
                ProductName: p.ProductName,
                BrandId: p.BrandId,
                Brand: p.SneakersBrand?.BrandName ?? string.Empty,
                Audience: p.TargetAudience.ToString(),
                AverageRating: p.AverageRating,
                Model: p.Model,
                Price: p.BasePrice,
                Images: p.ImagesUrls?.ToList() ?? [],
                IsActive: p.IsActive,
                DiscountType: p.Discounts.FirstOrDefault(d => d.IsActive())?.TypeDiscount.ToString(),
                Discount: p.Discounts.FirstOrDefault(d => d.IsActive())?.DiscountValue
            )).ToList();

            return new GetProductsResponse(productDtos, totalCount);
        }
    }
}
