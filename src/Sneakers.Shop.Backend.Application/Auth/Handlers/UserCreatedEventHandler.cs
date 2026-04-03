using MediatR;
using Sneakers.Shop.Backend.Application.Common;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Events;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Auth.Handlers
{
    public class UserCreatedEventHandler(
        IUserProfileRepository userProfile, 
        IUnitOfWork unitOfWork)
         : INotificationHandler<DomainEventNotification<UserCreatedEvent>>
    {
        private readonly IUserProfileRepository _userProfile = userProfile;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(
            DomainEventNotification<UserCreatedEvent> notification, 
            CancellationToken cancellationToken)
        {
            var @event = notification.DomainEvent;
            var userProfileEntity = new UserProfile(
                @event.UserId,
                @event.Name,
                @event.Lastname,
                @event.PhoneNumber,
                @event.Email);

            await _userProfile.AddAsync(userProfileEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
