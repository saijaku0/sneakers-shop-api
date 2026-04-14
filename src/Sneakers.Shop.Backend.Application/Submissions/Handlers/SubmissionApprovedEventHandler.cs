using MediatR;
using Sneakers.Shop.Backend.Application.Common;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Events;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Handlers
{
    public class SubmissionApprovedEventHandler(
        IProductSubmissionRepository submissionRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
        : INotificationHandler<DomainEventNotification<SubmissionApprovedEvent>>
    {
        private readonly IProductSubmissionRepository _submissionRepository = submissionRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task Handle(
            DomainEventNotification<SubmissionApprovedEvent> notification,
            CancellationToken cancellationToken)
        {
            var submission = await _submissionRepository.GetByIdAsync(notification
                .DomainEvent
                .SubmissionId,
                cancellationToken);
            if (submission is null)
                return;
            var newProduct = new Product(
                submission.BrandId,
                submission.TargetAudience,
                submission.ProductName,
                submission.Model,
                submission.Description,
                submission.BasePrice);
            newProduct.UpdateProductPhoto([.. submission.ImagesUrls]);
            await _productRepository.AddAsync(newProduct, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
