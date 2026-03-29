using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class DropperPayout : Entity
    {
        public Guid DropperId { get; private set; }
        public Guid ProductId { get; private set; }
        public decimal Amount { get; private set; }
        public PayoutStatus Status { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? PaidAt { get; private set; }

        private DropperPayout() { }

        public DropperPayout(
            Guid dropperId, 
            Guid productId, 
            decimal amount) : base(Guid.NewGuid())
        {
            if (dropperId == Guid.Empty)
                throw new DomainException("DropperId cannot be empty.", nameof(dropperId));
            if (productId == Guid.Empty)
                throw new DomainException("ProductId cannot be empty.", nameof(productId));
            if (amount < 0)
                throw new DomainException("Amount cannot be negative.", nameof(amount));

            DropperId = dropperId;
            ProductId = productId;
            Amount = amount;
            Status = PayoutStatus.Pending;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public void MarkAsPaid()
        {
            if (Status != PayoutStatus.Pending)
                throw new DomainException("Only pending payouts can be paid.");

            Status = PayoutStatus.Completed;
            PaidAt = DateTimeOffset.UtcNow;
        }

        public void Cancel()
        {
            if (Status == PayoutStatus.Completed)
                throw new DomainException("Cannot cancel a completed payout.");

            Status = PayoutStatus.Cancelled;
        }

        public void MarkAsFailed()
        {
            if (Status != PayoutStatus.Pending)
                throw new DomainException("Only pending payouts can be marked as failed.");
            Status = PayoutStatus.Failed;
        }

        public void UpdateAmount(decimal newAmount)
        {
            if (Status != PayoutStatus.Pending)
                throw new DomainException("Only pending payouts can be updated.");
            if (newAmount < 0)
                throw new DomainException("Amount cannot be negative.", nameof(newAmount));
            Amount = newAmount;
        }

        public void UpdateProductId(Guid newProductId)
        {
            if (Status != PayoutStatus.Pending)
                throw new DomainException("Only pending payouts can be updated.");
            if (newProductId == Guid.Empty)
                throw new DomainException("ProductId cannot be empty.", nameof(newProductId));
            ProductId = newProductId;
        }
    }
}
