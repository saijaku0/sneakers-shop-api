using MediatR;
using Sneakers.Shop.Backend.Application.Common;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Events;
using Sneakers.Shop.Backend.Domain.Exceptions;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Handlers
{
    public class SubmissionApprovedEventHandler(
        IProductSubmissionRepository submissionRepository,
        IProductRepository productRepository,
        ISizeRepository sizeRepository,
        IUnitOfWork unitOfWork)
        : INotificationHandler<DomainEventNotification<SubmissionApprovedEvent>>
    {
        private readonly IProductSubmissionRepository _submissionRepository = submissionRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly ISizeRepository _sizeRepository = sizeRepository;
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

            var sizesInCm = submission.SubmissionSizes
                .Select(s => s.SizeInCm)
                .Distinct()
                .ToList();
            var sizesDictionary = await _sizeRepository.GetBySizesInCmAsync(sizesInCm, cancellationToken);

            foreach (var subSize in submission.SubmissionSizes)
            {
                var sizeEntity = sizesDictionary.FirstOrDefault(s => s.Value == subSize.SizeInCm)
                    ?? throw new DomainException($"Size {subSize.SizeInCm} not found in global dictionary!");

                var warehouseItem = new WarehouseItem(newProduct.Id, sizeEntity.Id, subSize.Quantity);
                newProduct.AddWarehouseItem(warehouseItem);
            }

            await _productRepository.AddAsync(newProduct, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
