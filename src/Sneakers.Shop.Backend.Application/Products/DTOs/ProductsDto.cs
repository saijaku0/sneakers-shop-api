namespace Sneakers.Shop.Backend.Application.Products.DTOs
{
    /// <summary>
    /// Represents product information, including name, brand, target audience, rating, model, price,
    /// images, active status, and discount details.
    /// </summary>
    /// <param name="ProductName">The name of the product. Cannot be null or empty.</param>
    /// <param name="BrandId">The unique identifier of the brand associated with the product.</param>
    /// <param name="Brand">The name of the brand to which the product belongs.</param>
    /// <param name="Audience">The target audience of the product, e.g., 'men', 'women', or 'children'.</param>
    /// <param name="AverageRating">The average rating of the product. Must be in the range from 0 to 5.</param>
    /// <param name="Model">The product model.</param>
    /// <param name="Price">The price of the product. Must be non-negative.</param>
    /// <param name="Images">A list of image URLs associated with the product. Can be empty if no images are available.</param>
    /// <param name="IsActive">True if the product is active and available for purchase; otherwise, false.</param>
    /// <param name="DiscountType">The type of discount applied, if any. Can be null if no discount is applied.</param>
    /// <param name="Discount">The discount value, if applied. Can be null if no discount is present.</param>
    public record ProductsDto(
        string ProductName,
        Guid BrandId,
        string Brand,
        string Audience,
        decimal AverageRating,
        string Model,
        decimal Price,
        List<string> Images,
        bool IsActive,
        string? DiscountType,
        decimal? Discount);
}
