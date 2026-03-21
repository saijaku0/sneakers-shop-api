using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class Discount
    {
        public Guid Id { get; private set; }
        public DiscontType DiscontType { get; private set; }
        public DateTimeOffset StartDate { get; private set; }
        public DateTimeOffset EndDate { get; private set; }
        public decimal DiscountValue { get; private set; }
        public Guid? ProductId { get; private set; }
        public Guid? BrandId { get; private set; }

        private Discount() { }

        public Discount(
            decimal value,
            DiscontType discount,
            DateTimeOffset start,
            DateTimeOffset end,
            Guid? productId,
            Guid? brandId)
        {
            if (start > end)
                throw new DomainException("Cannot create discont which end before start.");
            if (discount == DiscontType.FixedAmount || value < 0)
                throw new DomainException($"Discont: {value}, cannot be less than zero.");
            if (discount == DiscontType.Percentage && (value > 100 || value < 0))
                throw new DomainException("Percentage discount must be between 0 and 100.", nameof(value));
            if (productId.HasValue == brandId.HasValue)
                throw new DomainException("Discount must be applied either to a product or to a brand.");

            Id = Guid.NewGuid();
            DiscountValue = value;
            DiscontType = discount;
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
