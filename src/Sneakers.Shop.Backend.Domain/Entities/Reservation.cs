using Sneakers.Shop.Backend.Domain.Abstractions;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;

namespace Sneakers.Shop.Backend.Domain.Entities
{
    public class Reservation : Entity
    {
        public Guid WarehouseItemId { get; private set; }
        public Guid UserId { get; private set; }
        public int Quantity { get; private set; }
        public ReservationStatus Status { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset ExpiresAt { get; private set; }
        private Reservation() { }

        public Reservation(
            Guid warehouseItemId, 
            Guid userId, 
            DateTimeOffset expiresAt, 
            int quantity) : base(Guid.NewGuid())
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be positive.", nameof(quantity));

            WarehouseItemId = warehouseItemId;
            UserId = userId;
            Status = ReservationStatus.Pending;
            CreatedAt = DateTimeOffset.UtcNow;
            ExpiresAt = expiresAt;
            Quantity = quantity;
        }

        public void Confirm()
        {
            if (!CanBeConfirmed())
                throw new DomainException("Only pending reservations can be confirmed.");
            Status = ReservationStatus.Confirmed;
        }

        public void Cancel()
        {
            if (!CanBeCancelled())
                throw new DomainException("Reservation cannot be cancelled.");
            Status = ReservationStatus.Cancelled;
        }

        public void Renew(TimeSpan additionalTime)
        {
            if (!CanBeModified())
                throw new DomainException("Only pending and non-expired reservations can be renewed.");
            ExpiresAt = ExpiresAt.Add(additionalTime);
        }

        public void Reduce(TimeSpan reductionTime)
        {
            if (!CanBeModified())
                throw new DomainException("Only pending and non-expired reservations can be reduced.");
            ExpiresAt = ExpiresAt.Subtract(reductionTime);
        }

        public void SetStatus(ReservationStatus newStatus)
        {
            if (newStatus == ReservationStatus.Expired && !IsExpired())
                throw new DomainException("Cannot set status to expired if reservation is not expired.");
            Status = newStatus;
        }

        public void UpdateExpiresAt(DateTimeOffset newExpiresAt)
        {
            if (!CanBeModified())
                throw new DomainException("Only pending and non-expired reservations can have their expiration time updated.");
            ExpiresAt = newExpiresAt;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (!CanBeModified())
                throw new DomainException("Only pending and non-expired reservations can have their quantity updated.");
            if (newQuantity <= 0)
                throw new DomainException("Quantity must be greater than zero.");
            Quantity = newQuantity;
        }

        public void UpdateWarehouseItemId(Guid newWarehouseItemId)
        {
            if (!CanBeModified())
                throw new DomainException("Only pending and non-expired reservations can have their warehouse item updated.");
            WarehouseItemId = newWarehouseItemId;
        }

        public void Expire()
        {
            if (IsExpired() && Status == ReservationStatus.Pending)
                Status = ReservationStatus.Expired;
        }

        public bool IsCancelled() => Status == ReservationStatus.Cancelled;
        public bool IsConfirmed() => Status == ReservationStatus.Confirmed;
        public bool IsPending() => Status == ReservationStatus.Pending;
        public bool IsExpired() => DateTimeOffset.UtcNow > ExpiresAt;
        public bool CanBeConfirmed() => Status == ReservationStatus.Pending && !IsExpired();
        public bool CanBeCancelled() => Status != ReservationStatus.Cancelled;
        public bool CanBeModified() => Status == ReservationStatus.Pending && !IsExpired();

    }
}
