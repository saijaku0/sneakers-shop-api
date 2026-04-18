using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class WarehouseItem : Entity
    {
        public Guid ProductId { get; private set; }
        public Guid SizeId { get; private set; }
        public int Quantity { get; private set; }
        public byte[]? RowVersion { get; private set; }

        private WarehouseItem() { }

        public WarehouseItem (
            Guid productId,
            Guid productSizeId,
            int initialQuantity) : base(Guid.NewGuid())
        {
            if (productId == Guid.Empty) 
                throw new DomainException("ProductId cannot be empty", nameof(productId));
            if (productSizeId == Guid.Empty) 
                throw new DomainException("SizeId cannot be empty", nameof(productSizeId));
            if (initialQuantity < 0) 
                throw new DomainException("Initial quantity cannot be negative", nameof(initialQuantity));

            ProductId = productId;
            SizeId = productSizeId;
            Quantity = initialQuantity;
        }

        public void DecreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new DomainException("Amount must be positive", nameof(amount));
            if (amount > Quantity)
                throw new DomainException($"Cannot decrease below zero. Available: {Quantity}");

            Quantity -= amount;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new DomainException("Amount must be positive", nameof(amount));

            Quantity += amount;
        }

        public void AdjustQuantity(int newQuantity)
        {
            if (newQuantity < 0)
                throw new DomainException("Quantity cannot be negative", nameof(newQuantity));

            Quantity = newQuantity;
        }

        internal void UpdateRowVersion(byte[] rowVersion)
        {
            RowVersion = rowVersion;
        }
    }
}
