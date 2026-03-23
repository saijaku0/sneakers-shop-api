using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class TargetAudience : Entity
    {
        public string Title { get; private set; } = string.Empty;
        public bool IsDeleted { get; private set; }
        public DateTimeOffset? DeleteAt { get; private set; }

        private TargetAudience() { }

        public TargetAudience(string title)
            : base(Guid.NewGuid())
        {
            IsDeleted = false;
            SetAudienceName(title);
        }

        public void SetAudienceName(string audience)
        {
            if (IsDeleted) throw new DomainException("You cannot change removed audince name");
            if (string.IsNullOrWhiteSpace(audience))
                throw new DomainException("Target audience cannot be empty");
            if (audience == Title)
                throw new DomainException("This audience is already exist");

            Title = audience;
        }

        public void RemoveAudience()
        {
            IsDeleted = true;
            DeleteAt = DateTimeOffset.UtcNow;
        }
    }
}
