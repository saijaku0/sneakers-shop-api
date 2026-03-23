using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class OrderStatusHistory : Entity
    {
        public Guid OrderId { get; private set; }
        public OrderStatus? OldStatus { get; private set; } 
        public OrderStatus NewStatus { get; private set; }
        public DateTimeOffset ChangedAt { get; private set; }
        public string? Comment { get; private set; }

        private OrderStatusHistory() { }

        public OrderStatusHistory(
            Guid orderId,
            OrderStatus newStatus,
            OrderStatus? oldStatus = null,
            string? comment = null) : base(Guid.NewGuid())
        {
            if (orderId == Guid.Empty)
                throw new DomainException("OrderId cannot be empty.", nameof(orderId));
            if (!Enum.IsDefined(newStatus))
                throw new DomainException("Invalid new status.", nameof(newStatus));
            if (oldStatus.HasValue && !Enum.IsDefined(oldStatus.Value))
                throw new DomainException("Invalid old status.", nameof(oldStatus));
            if (oldStatus.HasValue && oldStatus.Value == newStatus)
                throw new DomainException("Old status and new status cannot be the same.");
            if (comment != null && comment.Length > 500)
                throw new DomainException("Comment cannot exceed 500 characters.", nameof(comment));

            OrderId = orderId;
            NewStatus = newStatus;
            OldStatus = oldStatus;
            ChangedAt = DateTimeOffset.UtcNow;
            Comment = comment;
        }
    }
}

