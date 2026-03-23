using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class Brand : Entity
    {
        public string BrandName { get; private set; } = string.Empty;
        public bool IsDeleted { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }

        private Brand() { }

        public Brand(string newName) : base(Guid.NewGuid())
        {
            IsDeleted = false;

            UpdateBrandName(newName);
        }

        public void UpdateBrandName(string name)
        {
            if (IsDeleted) throw new DomainException("You cannot change removed brand name");
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("New brand name cannot be empty");

            BrandName = name;
        }

        public void DeleteBrand()
        {
            IsDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;
        }
    }
}
