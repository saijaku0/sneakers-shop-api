using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class OrderStatusHistory : IEquatable<OrderStatusHistory>
    {
        public Guid Id { get; private set; }
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
            string? comment = null)
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

            Id = Guid.NewGuid();
            OrderId = orderId;
            NewStatus = newStatus;
            OldStatus = oldStatus;
            ChangedAt = DateTimeOffset.UtcNow;
            Comment = comment;
        }
        
        public override bool Equals(object? obj) => obj is OrderStatusHistory other && Id.Equals(other.Id);
        public override int GetHashCode() => Id.GetHashCode();
        public bool Equals(OrderStatusHistory? other) => other != null && Id.Equals(other.Id);
    }
}

