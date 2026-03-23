using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class Discount : Entity
    {
        public DiscountType TypeDiscount { get; private set; }
        public DateTimeOffset StartDate { get; private set; }
        public DateTimeOffset EndDate { get; private set; }
        public decimal DiscountValue { get; private set; }
        public Guid? ProductId { get; private set; }
        public Guid? BrandId { get; private set; }

        private Discount() { }

        public Discount(
            decimal value,
            DiscountType discount,
            DateTimeOffset start,
            DateTimeOffset end,
            Guid? productId,
            Guid? brandId) : base(Guid.NewGuid())
        {
            if (start > end)
                throw new DomainException("Cannot create discont which end before start.");
            if (discount == DiscountType.FixedAmount && value < 0)
                throw new DomainException($"Discont: {value}, cannot be less than zero.");
            if (discount == DiscountType.Percentage && (value > 100 || value < 0))
                throw new DomainException("Percentage discount must be between 0 and 100.", nameof(value));
            if (productId == Guid.Empty || brandId == Guid.Empty)
                throw new DomainException("Brand ID and product ID cannot be empty.");
            if (productId.HasValue == brandId.HasValue)
                throw new DomainException("Discount must be applied either to a product or to a brand.");

            DiscountValue = value;
            TypeDiscount = discount;
            StartDate = start;
            EndDate = end;
            ProductId = productId;
            BrandId = brandId;
        }

        public bool IsActive()
        {
            var now = DateTimeOffset.UtcNow;
            return now >= StartDate && now <= EndDate;
        }

        public void DeactivateEarly()
        {
            var now = DateTimeOffset.UtcNow;

            if (!IsActive())
                throw new DomainException("Discount is already inactive.");

            EndDate = now;
        }
    }
}
