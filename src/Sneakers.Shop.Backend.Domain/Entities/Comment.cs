using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class Comment : Entity
    {
        public Guid ProductId { get; private set; }
        public Guid UserId { get; private set; }
        public string? Description { get; private set; }
        public decimal Rating {  get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        private Comment () { }

        public Comment(
            Guid productId,
            Guid userId,
            string? description,
            decimal rating) : base(Guid.NewGuid())
        {
            if (rating <= 0 || rating > 5)
                throw new DomainException("Rating cannot be zero or less and greater than 5");
            if (productId == Guid.Empty)
                throw new DomainException("Product ID cannot be empty");
            if (userId == Guid.Empty)
                throw new DomainException("User ID cannot be empty");

            ProductId = productId;
            UserId = userId;
            Description = DescriptionValidation(description);
            Rating = rating;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        static private string DescriptionValidation(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return string.Empty;

            int descriptionLength = description.Length;

            if (descriptionLength > 1000)
                throw new DomainException("Description length must not exceed 1000 characters.");

            return description;
        }
    }
}
