using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class WarehouseItem : Entity
    {
        public Guid ProductId { get; private set; }
        public Guid SizeId { get; private set; }
        public int Quantity { get; private set; }
        public int Reserved { get; private set; }
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
            Reserved = 0;
        }

        public int AvailableQuantity => Quantity - Reserved;

        static private bool IsAmountZeroOrNegative(int amount) => amount <= 0;
        static private bool IsAmountGreaterThanZero(int amount) => amount > 0;
        private bool IsAmountGreaterThanReserved(int amount) => amount > Reserved;
        private bool IsAmountLessOrEqualAvailableQuantity(int amount) => amount <= AvailableQuantity;

        public void Reserve(int amount)
        {
            if(!IsAmountGreaterThanZero(amount))
                throw new DomainException($"Cannot reserve {amount} items.");
            if (!IsAmountLessOrEqualAvailableQuantity(amount))
                throw new DomainException($"Cannot set amount bigger than reserved. Available: {AvailableQuantity}");

            Reserved += amount; 
        }

        public void Unreserve (int amount)
        {
            if (IsAmountZeroOrNegative(amount))
                throw new DomainException("Amount must be positive", nameof(amount));
            if (IsAmountGreaterThanReserved(amount))
                throw new DomainException($"Cannot unreserve {amount} items. Reserved: {Reserved}");

            Reserved -= amount;
        }

        public void FulfillReserved(int amount)
        {
            if (IsAmountZeroOrNegative(amount)) 
                throw new DomainException("Amount must be positive", nameof(amount));
            if (IsAmountGreaterThanReserved(amount)) 
                throw new DomainException($"Cannot fulfill {amount} items. Reserved: {Reserved}");

            Reserved -= amount;
            Quantity -= amount;
        }

        public void IncreaseQuantity(int amount)
        {
            if (IsAmountZeroOrNegative(amount))
                throw new DomainException("Amount must be positive", nameof(amount));

            Quantity += amount;
        }

        public void AdjustQuantity(int newQuantity)
        {
            if (newQuantity < 0) 
                throw new DomainException("Quantity cannot be negative", nameof(newQuantity));
            if (newQuantity < Reserved) 
                throw new DomainException($"Cannot set quantity less than reserved ({Reserved})");

            Quantity = newQuantity;
        }

        internal void UpdateRowVersion(byte[] rowVersion) 
        {
            RowVersion = rowVersion;
        }
    }
}
