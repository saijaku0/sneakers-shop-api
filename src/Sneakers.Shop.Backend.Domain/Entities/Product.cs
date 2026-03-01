using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public Guid BrandId { get; private set; }
        public Brand? SneakersBrand { get; private set; }

        public IReadOnlyCollection<WarehouseItem> WarehouseItems => _warehouseItems.AsReadOnly();
        private readonly List<WarehouseItem> _warehouseItems = [];
        
        public string ProductName { get; private set; } = string.Empty;
        public string Model { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal BasePrice { get; private set; }

        public List<string>? ImagesUrls { get; private set; }
        public bool IsActive {  get; private set; }
        public DateTimeOffset CreateAt {  get; private set; }

        private Product() { }

        public Product(
            Guid brandId,
            string productName,
            string model,
            string description,
            decimal basePrice)
        {
            if (basePrice < 0)
                throw new DomainException("Base price cannot be less then 0");

            ProductName = productName;
            BrandId = brandId;
            Model = model;
            Description = description;
            BasePrice = basePrice;

            Id = Guid.NewGuid();
            CreateAt = DateTimeOffset.UtcNow;
        }

        //TO DO: Add Behaviors
    }
}
