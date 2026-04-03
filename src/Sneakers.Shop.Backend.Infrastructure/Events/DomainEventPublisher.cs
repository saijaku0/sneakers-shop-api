using MediatR;
using Sneakers.Shop.Backend.Application.Common;
using Sneakers.Shop.Backend.Application.Common.Interfaces;
using Sneakers.Shop.Backend.Domain.Events;

namespace Sneakers.Shop.Backend.Infrastructure.Events
{
    public class DomainEventPublisher(IMediator mediator) : IDomainEventPublisher
    {
        private readonly IMediator _mediator = mediator;
        public async Task PublishAsync(IDomainEvent domainEvent, CancellationToken ct = default)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = Activator.CreateInstance(notificationType, domainEvent);
            await _mediator.Publish(notification!, ct);
        }
    }
}
