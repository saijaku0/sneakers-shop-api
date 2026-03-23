using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class PromoCode : Entity
    {
        public Guid? UserId { get; private set; }
        public Guid? ProductId { get; private set; }
        public DiscountType DiscountType { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public decimal DiscountValue { get; private set; }
        public bool IsUsed { get; private set; }
        public DateTimeOffset ValidFrom { get; private set; }
        public DateTimeOffset ExpirationDate { get; private set; }

        private PromoCode() { }

        public PromoCode(
            string code,
            DiscountType discountType,
            decimal discountValue,
            DateTimeOffset validFrom,
            DateTimeOffset expirationDate,
            Guid? userId = null,
            Guid? productId = null) : base(Guid.NewGuid())
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new DomainException("Promo code cannot be empty.", nameof(code));
            if (discountValue <= 0)
                throw new DomainException("Discount value must be positive.", nameof(discountValue));
            if (validFrom >= expirationDate)
                throw new DomainException("ValidFrom must be earlier than ExpirationDate.");
            if (validFrom < DateTimeOffset.UtcNow.Date)
                throw new DomainException("ValidFrom cannot be in the past.");

            if (discountType == DiscountType.Percentage && discountValue > 100)
                throw new DomainException("Percentage discount cannot exceed 100.");
            if (discountType == DiscountType.FixedAmount && discountValue <= 0)
                throw new DomainException("Fixed amount discount must be positive.");

            Code = code.Trim().ToUpperInvariant(); 
            DiscountType = discountType;
            DiscountValue = discountValue;
            ValidFrom = validFrom;
            ExpirationDate = expirationDate;
            UserId = userId;
            ProductId = productId;
            IsUsed = false;
        }

        public bool IsActive => !IsUsed 
            && DateTimeOffset.UtcNow >= ValidFrom
            && DateTimeOffset.UtcNow <= ExpirationDate;

        public void UseCode()
        {
            if (IsUsed)
                throw new DomainException("Promo code has already been used.");
            if (!IsActive)
                throw new DomainException("Promo code is not active (expired or not yet valid).");

            IsUsed = true;
        }

        public void Unuse()
        {
            if (!IsUsed)
                throw new DomainException("Promo code is not used.");
            if (DateTimeOffset.UtcNow >= ExpirationDate)
                throw new DomainException("Promo code is expired.");

            IsUsed = false;
        }

        public void UpdateValidityPeriod(
            DateTimeOffset newValidFrom, 
            DateTimeOffset newExpirationDate)
        {
            if (IsUsed)
                throw new DomainException("Cannot modify validity period of a used promo code.");
            if (newValidFrom >= newExpirationDate)
                throw new DomainException("ValidFrom must be earlier than ExpirationDate.");
            if (newValidFrom < DateTimeOffset.UtcNow.Date)
                throw new DomainException("ValidFrom cannot be in the past.");

            ValidFrom = newValidFrom;
            ExpirationDate = newExpirationDate;
        }

        public void UpdateDiscount(decimal newDiscountValue)
        {
            if (IsUsed)
                throw new DomainException("Cannot modify discount of a used promo code.");
            if (newDiscountValue <= 0)
                throw new DomainException("Discount value must be positive.", nameof(newDiscountValue));
            if (DiscountType == DiscountType.Percentage && newDiscountValue > 100)
                throw new DomainException("Percentage discount cannot exceed 100.");

            DiscountValue = newDiscountValue;
        }

        public bool IsApplicable(Guid userId, Guid productId)
        {
            return IsActive &&
                   (UserId == null || UserId == userId) &&
                   (ProductId == null || ProductId == productId);
        }
    }
}
