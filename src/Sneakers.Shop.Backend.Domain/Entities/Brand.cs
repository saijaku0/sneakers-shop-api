using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class Brand
    {
        public Guid Id { get; private set; }
        public string BrandName { get; private set; } = string.Empty;

        public Brand() { }

        public Brand(string newName)
        {
            UpdateBrandName(newName);
            Id = Guid.NewGuid();
        }

        public void UpdateBrandName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("New brand name cannot be empty");

            BrandName = name;
        }
    }
}
