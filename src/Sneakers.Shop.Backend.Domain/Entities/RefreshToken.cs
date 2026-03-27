using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class RefreshToken : Entity
    {
        public string Token { get; private set; } = string.Empty;
        public Guid UserId { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset ExpiresAt {  get; private set; }
        public bool IsRevoked { get; private set; }

        private RefreshToken() { }

        public RefreshToken(
            Guid userId,
            string token,
            DateTimeOffset expiresAt) : base(Guid.NewGuid()) 
        {
            if (userId == Guid.Empty) 
                throw new DomainException("User ID cannot be empty.");
            if (string.IsNullOrWhiteSpace(token)) 
                throw new DomainException("Token cannot be empty.");
            if (expiresAt <= DateTimeOffset.UtcNow) 
                throw new DomainException("Expiration must be in the future.");

            UserId = userId;
            Token = token;
            CreatedAt = DateTimeOffset.UtcNow;
            ExpiresAt = expiresAt;
            IsRevoked = false;
        }

        public void Revoke()
        {
            if (IsRevoked)
                throw new DomainException("Revoke is already true");

            IsRevoked = true;
        }
        public bool IsValid() => !IsRevoked && ExpiresAt > DateTimeOffset.UtcNow;
    }
}
