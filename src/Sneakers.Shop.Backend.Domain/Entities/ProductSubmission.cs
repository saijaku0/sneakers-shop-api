using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;
using Sneakers.Shop.Backend.Domain.ValueObjects;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class ProductSubmission : Entity
    {
        private readonly List<SubmissionSize> _submissionSizes = [];
        public Guid DropId { get; private set; }
        public Guid BrandId { get; private set; }
        public Brand? SneakersBrand { get; private set; }
        public Audience TargetAudience { get; private set; }
        public IReadOnlyCollection<SubmissionSize> SubmissionSizes => _submissionSizes.AsReadOnly();

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
            decimal basePrice,
            IEnumerable<(int quantity, decimal sizeInCm)> submissionSizes) : base(Guid.NewGuid())
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
            if (submissionSizes == null || !submissionSizes.Any())
                throw new DomainException("At least one submission size must be provided.", nameof(submissionSizes));

            foreach (var (quantity, sizeInCm) in submissionSizes)
                _submissionSizes.Add(new SubmissionSize(quantity, sizeInCm));

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

        private void EnsurePendingStatus()
        {
            if (Status != ProductSubmissionStatus.Pending)
                throw new DomainException($"Operation not allowed in current status '{Status}'. Only 'Pending' allowed.");
        }

        private void EnsureModeratorId(Guid moderatorId)
        {
            if (moderatorId == Guid.Empty)
                throw new DomainException("ModeratorId cannot be empty.", nameof(moderatorId));
        }

        public void Approve(Guid moderatorId)
        {
            EnsureModeratorId(moderatorId);
            EnsurePendingStatus();

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
            EnsurePendingStatus();

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
            EnsurePendingStatus();

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
            EnsurePendingStatus();

            Status = ProductSubmissionStatus.Cancelled;
        }

        public void AddSize(int quantity, decimal sizeInCm)
        {
            EnsurePendingStatus();
            var newSize = new SubmissionSize(quantity, sizeInCm);
            if (_submissionSizes.Any(s => s.SizeInCm == sizeInCm))
                throw new DomainException($"Size with {sizeInCm} cm already exists in submission.");
            _submissionSizes.Add(newSize);
        }

        public void RemoveSize(decimal sizeInCm)
        {
            EnsurePendingStatus();
            var sizeToRemove = _submissionSizes.FirstOrDefault(s => s.SizeInCm == sizeInCm)
                ?? throw new DomainException($"Size with {sizeInCm} cm not found in submission.");
            if (_submissionSizes.Count == 1)
                throw new DomainException("Cannot remove the last size. Submission must have at least one size.");
            _submissionSizes.Remove(sizeToRemove);
        }

        public void UpdateQuantityForSize(decimal sizeInCm, int newQuantity)
        {
            EnsurePendingStatus();
            var existingSize = _submissionSizes.FirstOrDefault(s => s.SizeInCm == sizeInCm)
                ?? throw new DomainException($"Size {sizeInCm} cm not found in this submission.");

            if (newQuantity <= 0)
                throw new DomainException("Quantity must be greater than zero.", nameof(newQuantity));

            var index = _submissionSizes.IndexOf(existingSize);
            _submissionSizes[index] = new SubmissionSize(newQuantity, sizeInCm);
        }

        public void SetSizeList(IEnumerable<(int quantity, decimal sizeInCm)> newSizes)
        {
            EnsurePendingStatus();
            if (newSizes == null || !newSizes.Any())
                throw new DomainException("At least one submission size must be provided.", nameof(newSizes));

            var updatedSizes = new List<SubmissionSize>();
            foreach (var (quantity, sizeInCm) in newSizes)
                updatedSizes.Add(new SubmissionSize(quantity, sizeInCm));

            if (updatedSizes.Select(s => s.SizeInCm).Distinct().Count() != updatedSizes.Count)
                throw new DomainException("Duplicate sizes are not allowed in submission.");

            _submissionSizes.Clear();
            _submissionSizes.AddRange(updatedSizes);
        }
    }
}
