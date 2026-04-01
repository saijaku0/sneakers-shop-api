using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class BrandDiscount : Discount
    {
        public Guid BrandId { get; private set; }
        protected BrandDiscount() { }
        public BrandDiscount(
            Guid brandId,
            decimal value,
            DiscountType discount,
            DateTimeOffset start,
            DateTimeOffset end) : base(value, discount, start, end)
        {
            if (brandId == Guid.Empty)
                throw new ArgumentException("BrandId cannot be empty.", nameof(brandId));

            BrandId = brandId;
        }
    }
}