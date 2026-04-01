using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class ProductDiscount : Discount
    {
        public Guid ProductId { get; private set; }
        protected ProductDiscount() { }
        public ProductDiscount(
            Guid productId,
            decimal value,
            DiscountType discount,
            DateTimeOffset start,
            DateTimeOffset end) : base(value, discount, start, end)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("ProductId cannot be empty.", nameof(productId));

            ProductId = productId;
        }
    }
}
