using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid WarehouseItemId { get; private set; }
        public WarehouseItem WarehouseItem { get; private set; } = null!;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public decimal TotalPrice => (UnitPrice - DiscountAmount) * Quantity;

        private OrderItem() { }

        public OrderItem(
            Guid orderId,
            Guid warehouseItemId,
            int quantity,
            decimal unitPrice,
            decimal discountAmount) : base(Guid.NewGuid())
        {
            if (orderId == Guid.Empty)
                throw new DomainException("OrderId cannot be empty.", nameof(orderId));
            if (warehouseItemId == Guid.Empty)
                throw new DomainException("WarehouseItemId cannot be empty.", nameof(warehouseItemId));
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero.", nameof(quantity));
            if (unitPrice < 0)
                throw new DomainException("UnitPrice cannot be negative.", nameof(unitPrice));
            if (discountAmount < 0)
                throw new DomainException("DiscountAmount cannot be negative.", nameof(discountAmount));
            if (discountAmount > unitPrice)
                throw new DomainException("DiscountAmount cannot exceed UnitPrice.", nameof(discountAmount));

            OrderId = orderId;
            WarehouseItemId = warehouseItemId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            DiscountAmount = discountAmount;
        }
    }
}
