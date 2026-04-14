using MediatR;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Events;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Handlers
{
    public class SubmissionApprovedEventHandler(
        IProductSubmissionRepository submissionRepository,
        IProductRepository productRepository) 
        : INotificationHandler<SubmissionApprovedEvent>
    {
        private readonly IProductSubmissionRepository _submissionRepository = submissionRepository;
        private readonly IProductRepository _productRepository = productRepository;
        public async Task Handle(SubmissionApprovedEvent notification, CancellationToken cancellationToken)
        {
            var submission = await _submissionRepository.GetByIdAsync(notification.SubmissionId, cancellationToken);
            if (submission is null)
                return;
            var newProduct = new Product(
                submission.BrandId,
                submission.TargetAudience,
                submission.ProductName,
                submission.Model,
                submission.Description,
                submission.BasePrice);
            await _productRepository.AddAsync(newProduct, cancellationToken);
        }
    }
}
