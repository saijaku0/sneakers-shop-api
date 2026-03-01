namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class WarehouseItem
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid SizeId { get; private set; }
        public int Quantity { get; private set; }
        public int Reserved { get; private set; }
        public byte[]? RowVersion { get; private set; }
    }
}
