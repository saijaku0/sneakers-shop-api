using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;
using Sneakers.Shop.Backend.Domain.ValueObjects;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class Order : Entity
    {
        public Guid UserId { get; private set; }
        public DateTimeOffset OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public Address ShippingAddress { get; private set; } = null!;

        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        private readonly List<OrderItem> _items = [];
        public IReadOnlyCollection<OrderStatusHistory> StatusHistory => _statusHistory.AsReadOnly();
        private readonly List<OrderStatusHistory> _statusHistory = [];

        public decimal TotalOrderPrice => _items.Sum(item => item.TotalPrice);

        private Order() { } 

        public Order(
            Guid userId,
            Address shippingAddress) : base(Guid.NewGuid())
        {
            if (userId == Guid.Empty)
                throw new DomainException("UserId cannot be empty.", nameof(userId));

            UserId = userId;
            OrderDate = DateTimeOffset.UtcNow;
            Status = OrderStatus.Pending;
            ShippingAddress = shippingAddress;

            _statusHistory.Add(new OrderStatusHistory(
                orderId: Id,
                newStatus: OrderStatus.Pending,
                oldStatus: null,
                comment: "Order created"));
        }

        public void AddOrderItem(
            Guid warehouseItemId,
            int quantity,
            decimal unitPrice,
            decimal discountAmount)
        {
            if (Status != OrderStatus.Pending)
                throw new DomainException($"Cannot add items to order in status '{Status}'. Only 'Pending' allowed.");

            var order = new OrderItem(
                orderId: Id,
                warehouseItemId: warehouseItemId,
                quantity: quantity,
                unitPrice: unitPrice,
                discountAmount: discountAmount);

            _items.Add(order);
        }

        public void ChangeStatus(OrderStatus newStatus, string? comment = null)
        {
            if (Status == OrderStatus.Cancelled)
                throw new DomainException("Cannot change status of a cancelled order.");
            if (Status == OrderStatus.Delivered)
                throw new DomainException("Cannot change status of a delivered order.");

            if (newStatus == Status)
                throw new DomainException($"Order is already in status '{Status}'.");

            var oldStatus = Status;

            Status = newStatus;

            _statusHistory.Add(new OrderStatusHistory(
                orderId: Id,
                newStatus: newStatus,
                oldStatus: oldStatus,
                comment: comment));
        }
    }
}
