using MediatR;
using Sneakers.Shop.Backend.Domain.Events;

namespace Sneakers.Shop.Backend.Application.Common
{
    public record DomainEventNotification<T>(T DomainEvent) 
        : INotification where T : IDomainEvent;
}
