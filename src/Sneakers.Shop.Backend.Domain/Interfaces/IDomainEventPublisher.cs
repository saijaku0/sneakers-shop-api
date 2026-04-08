using Sneakers.Shop.Backend.Domain.Events;

namespace Sneakers.Shop.Backend.Application.Common.Interfaces
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync(IDomainEvent domainEvent, CancellationToken ct = default);
    }
}
