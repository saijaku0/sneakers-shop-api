using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class ProductSubmission : Entity
    {
        public Guid DropId { get; private set; }
        public Guid BrandId { get; private set; }
        public Brand? SneakersBrand { get; private set; }
        public Audience TargetAudience { get; private set; }

        public string ProductName { get; private set; } = string.Empty;
        public string Model { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal BasePrice { get; private set; }

        public ProductSubmissionStatus Status { get; private set; }
        public string? RejectionReason { get; private set; }
        public Guid? ModeratorId { get; private set; }
        public DateTimeOffset CheckedAt { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        private ProductSubmission() { }

        public ProductSubmission(
            Guid dropId,
            Guid brandId,
            Audience audience,
            string productName,
            string model,
            string description,
            decimal basePrice) : base(Guid.NewGuid())
        {
            if (dropId == Guid.Empty)
                throw new DomainException("DropId cannot be empty.", nameof(dropId));
            if (brandId == Guid.Empty)
                throw new DomainException("BrandId cannot be empty.", nameof(brandId));
            if (string.IsNullOrWhiteSpace(productName))
                throw new DomainException("Product name cannot be empty.", nameof(productName));
            if (string.IsNullOrWhiteSpace(model))
                throw new DomainException("Model cannot be empty.", nameof(model));
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description cannot be empty.", nameof(description));
            if (basePrice < 0)
                throw new DomainException("Base price cannot be negative.", nameof(basePrice));

            DropId = dropId;
            BrandId = brandId;
            TargetAudience = audience;
            ProductName = productName;
            Model = model;
            Description = description;
            BasePrice = basePrice;
            Status = ProductSubmissionStatus.Pending;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public void Approve(Guid moderatorId)
        {
            if (moderatorId == Guid.Empty)
                throw new DomainException("ModeratorId cannot be empty.", nameof(moderatorId));
            if (Status != ProductSubmissionStatus.Pending)
                throw new DomainException($"Cannot approve submission in status '{Status}'. Only 'Pending' allowed.");

            Status = ProductSubmissionStatus.Approved;
            ModeratorId = moderatorId;
            CheckedAt = DateTimeOffset.UtcNow;
            RejectionReason = null;
        }

        public void Reject(Guid moderatorId, string reason)
        {
            if (moderatorId == Guid.Empty)
                throw new DomainException("ModeratorId cannot be empty.", nameof(moderatorId));
            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("Rejection reason cannot be empty.", nameof(reason));
            if (reason.Length > 500)
                throw new DomainException("Rejection reason cannot exceed 500 characters.", nameof(reason));
            if (Status != ProductSubmissionStatus.Pending)
                throw new DomainException($"Cannot reject submission in status '{Status}'. Only 'Pending' allowed.");

            Status = ProductSubmissionStatus.Rejected;
            ModeratorId = moderatorId;
            CheckedAt = DateTimeOffset.UtcNow;
            RejectionReason = reason.Trim();
        }

        public void UpdateDetails(
            Audience targetAudience,
            Guid brandId,
            string productName, 
            string model, 
            string description, 
            decimal basePrice)
        {
            if (Status != ProductSubmissionStatus.Pending)
                throw new DomainException($"Cannot update submission in status '{Status}'. Only 'Pending' allowed.");

            if (string.IsNullOrWhiteSpace(productName))
                throw new DomainException("Product name cannot be empty.", nameof(productName));
            if (string.IsNullOrWhiteSpace(model))
                throw new DomainException("Model cannot be empty.", nameof(model));
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description cannot be empty.", nameof(description));
            if (basePrice < 0)
                throw new DomainException("Base price cannot be negative.", nameof(basePrice));
            if (brandId == Guid.Empty)
                throw new DomainException("BrandId cannot be empty.", nameof(brandId));

            TargetAudience = targetAudience;
            BrandId = brandId;
            ProductName = productName.Trim();
            Model = model.Trim();
            Description = description.Trim();
            BasePrice = basePrice;
        }

        public void Cancel()
        {
            if (Status != ProductSubmissionStatus.Pending)
                throw new DomainException($"Cannot cancel submission in status '{Status}'. Only 'Pending' allowed.");

            Status = ProductSubmissionStatus.Cancelled;
        }
    }
}
