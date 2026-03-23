using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class ModerationLog
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public ModerationAction Action { get; private set; }
        public string Reason { get; private set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; private set; }

        private ModerationLog() { }

        public ModerationLog(
            Guid userId, 
            ModerationAction action, 
            string reason)
        {
            if (userId == Guid.Empty)
                throw new DomainException("UserId cannot be empty.", nameof(userId));
            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("Reason cannot be empty.", nameof(reason));
            if (reason.Length > 500)
                throw new DomainException("Reason cannot exceed 500 characters.", nameof(reason));

            Id = Guid.NewGuid();
            UserId = userId;
            Action = action;
            Reason = reason;
            CreatedAt = DateTimeOffset.UtcNow;

        }
    }
}
