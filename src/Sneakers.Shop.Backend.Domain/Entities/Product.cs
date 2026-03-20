using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public Guid BrandId { get; private set; }
        public Brand? SneakersBrand { get; private set; }
        public Guid AudienceId { get; private set; }
        public TargetAudience? TargetAudience { get; private set; }

        public IReadOnlyCollection<WarehouseItem> WarehouseItems => _warehouseItems.AsReadOnly();
        private readonly List<WarehouseItem> _warehouseItems = [];

        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
        private readonly List<Comment> _comments = [];

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
            Guid audienceId,
            string productName,
            string model,
            string description,
            decimal basePrice)
        {
            IsActive = true;

            UpdateBrand(brandId);
            UpdateAudience(audienceId);
            UpdateProductModel(model);
            UpdateProductDescAndName(description, productName);
            UpdateProductPrice(basePrice);

            Id = Guid.NewGuid();
            CreateAt = DateTimeOffset.UtcNow;
        }

        public void UpdateProductDescAndName(
            string description, 
            string productName)
        {
            if (!IsActive)
                throw new DomainException("You cannot modify deactivated product");
            if (string.IsNullOrWhiteSpace(productName))
                throw new DomainException("Product name cannot be empty");
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description cannot be empty");

            ProductName = productName;
            Description = description;
        }

        public void UpdateProductPrice(decimal price)
        {
            if (!IsActive) 
                throw new DomainException("You cannot modify deactivated product");
            if (price < 0) throw new DomainException("Price cannot be less than 0");

            BasePrice = price;
        }

        public void UpdateProductModel(string newModel)
        {
            if (!IsActive) throw new DomainException("You cannot modify deactivated product");
            if (string.IsNullOrWhiteSpace(newModel))
                throw new DomainException("Model cannot be empty");

            Model = newModel;
        }

        public void UpdateProductPhoto(List<string> photosUrls)
        {
            if (!IsActive) throw new DomainException("You cannot modify deactivated product");
            if (photosUrls is null)
                throw new DomainException("Photos cannot be empty");

            ImagesUrls = photosUrls;
        }

        public void UpdateBrand (Guid newBrandId)
        {
            if (!IsActive) throw new DomainException("You cannot modify deactivated product");
            if (newBrandId == Guid.Empty) throw new DomainException("Brand ID cannot be empty");

            BrandId = newBrandId;
        }

        public void UpdateAudience(Guid newAudienceId)
        {
            if (!IsActive) throw new DomainException("You cannot modify deactivated product");
            if (newAudienceId == Guid.Empty) throw new DomainException("Audience ID cannot be empty");

            AudienceId = newAudienceId;
        }

        public void UpdateProductStatus(bool status)
        {
            if (status == IsActive)
                throw new DomainException($"Cannot update status product already have this status: {IsActive}");

            IsActive = status;
        }
    }
}
