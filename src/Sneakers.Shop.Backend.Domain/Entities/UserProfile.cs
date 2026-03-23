using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;
using Sneakers.Shop.Backend.Domain.ValueObjects;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class UserProfile : Entity
    {
        public string Email { get; private set; } = string.Empty;
        public bool IsFlagged { get; private set; }
        public int WarningCount { get; private set; }
        public IReadOnlyCollection<WishlistItem> WishlistItems => _wishlistItems.AsReadOnly();
        private readonly List<WishlistItem> _wishlistItems = [];
        public IReadOnlyCollection<ModerationLog> ModerationLogs => _moderationLogs.AsReadOnly();
        private readonly List<ModerationLog> _moderationLogs = [];
        public bool IsDeleted { get; private set; }
        public Address? DefaultShippingAddress { get; private set; }
        public DateTimeOffset RegistrationDate { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }

        private UserProfile() { }

        public UserProfile(
            Guid userId,
            string email) : base(Guid.NewGuid())
        {
            if (userId == Guid.Empty)
                throw new DomainException("User ID cannot be empty");
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("User email cannot be empty");

            Id = userId;
            Email = email;
            WarningCount = 0;
            IsFlagged = false;
            RegistrationDate = DateTimeOffset.UtcNow;
            IsDeleted = false;
            DeletedAt = null;
        }

        private void CheckIfUserDeleted(string message)
        {
            if (IsDeleted) throw new DomainException(message);
        }

        public void UpdateDefaultAddress(Address newAddress)
        {
            CheckIfUserDeleted("User is deleted.");

            DefaultShippingAddress = newAddress;
        }

        public void ResetWarnings()
        {
            WarningCount = 0;
            IsFlagged = false;
        }

        public void DeleteProfile()
        {
            CheckIfUserDeleted("User already deleted.");

            IsDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;
        }

        public void ClearWishlist()
        {
            CheckIfUserDeleted("Cannot modify a deleted profile.");

            foreach (var item in _wishlistItems.Where(w => !w.IsDeleted))
                item.Remove();
        }

        public void Unflag(string reason)
        {
            if (IsDeleted)
                throw new DomainException("Cannot modify a deleted profile.");
            if (!IsFlagged)
                throw new DomainException("User is not flagged.");

            IsFlagged = false;
            _moderationLogs.Add(new ModerationLog(Id, ModerationAction.Unflagged, reason));
        }

        public void IssueWarning(string reason)
        {
            if (IsFlagged)
                throw new DomainException("User is already flagged. Cannot issue more warnings.");

            CheckIfUserDeleted("User is deleted.");
            _moderationLogs.Add(new ModerationLog(Id, ModerationAction.WarningIssued, reason));

            WarningCount++;

            if (WarningCount >= 3)
            {
                IsFlagged = true;
                _moderationLogs.Add(new ModerationLog(Id, ModerationAction.Flagged, "Automatically flagged after 3 warnings."));
            }
        }
    }
}
