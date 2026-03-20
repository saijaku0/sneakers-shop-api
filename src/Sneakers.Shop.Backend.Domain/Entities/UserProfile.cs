using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class UserProfile
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public bool IsFlagged { get; private set; }
        public int WarningCount { get; private set; }
        public DateTimeOffset RegistrationDate { get; private set; }

        private UserProfile() { }

        public UserProfile(
            Guid userId,
            string email)
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
        }

        public void IssueWarning()
        {
            WarningCount++;

            if (WarningCount >= 3)
                IsFlagged = true;
        }

        public void ResetWarnings()
        {
            WarningCount = 0;
            IsFlagged = false;
        }
    }
}
