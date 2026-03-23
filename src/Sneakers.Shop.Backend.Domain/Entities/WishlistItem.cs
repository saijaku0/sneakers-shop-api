using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class WishlistItem : Entity
    {
        public Guid ProductId { get; private set; }
        public Guid UserId { get; private set; }
        public Product? Product { get; private set; }
        public DateTimeOffset AddedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }

        private WishlistItem() { }

        public WishlistItem(
            Guid userId,
            Guid productId) : base(Guid.NewGuid())
        {
            if (userId == Guid.Empty)
                throw new DomainException("User ID cannot be empty");
            if (productId == Guid.Empty)
                throw new DomainException("Product ID cannot be empty");

            UserId = userId;
            ProductId = productId;
            AddedAt = DateTimeOffset.UtcNow;
            IsDeleted = false;
            DeletedAt = null;
        }

        public void Remove()
        {
            if (IsDeleted)
                throw new DomainException("Wishlist item is already deleted.");

            IsDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;
        }

        public void Restore()
        {
            if (!IsDeleted)
                throw new DomainException("Cannot restore a non-deleted wishlist item.");

            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
