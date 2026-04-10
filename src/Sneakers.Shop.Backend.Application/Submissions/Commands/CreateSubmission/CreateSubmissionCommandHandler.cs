using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Entities;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Interfaces;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.CreateSubmission
{
    public class CreateSubmissionCommandHandler(
        IBrandRepository brandRepository,
        IProductSubmissionRepository productRepository,
        ISizeConversionService sizeConversion,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateSubmissionCommand, Result<Guid>>
    {
        private readonly IBrandRepository _brandRepository = brandRepository;
        private readonly IProductSubmissionRepository _productRepository = productRepository;
        private readonly ISizeConversionService _sizeConversion = sizeConversion;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result<Guid>> Handle(
            CreateSubmissionCommand request, 
            CancellationToken cancellationToken)
        { 
            var brand = await _brandRepository.GetByIdAsync(request.BrandId, cancellationToken);
            if (brand == null)
                return Result<Guid>.Failure(Error.Conflict($"Brand with ID {request.BrandId} does not exist."));

            var size = request.SubmissionSizes.Select(s => {
                var sizeInCm = _sizeConversion.GetEquivalentSize(
                    s.SizeValue,
                    s.MeasureType,
                    MeasureSizes.CM,
                    request.TargetAudience
                );
                return (s.Quantity, sizeInCm);
            }).ToList();

            var submission = new ProductSubmission
            (
                request.DropId,
                request.BrandId,
                request.TargetAudience,
                request.ProductName,
                request.Model,
                request.Description,
                request.BasePrice,
                size
            );
            await _productRepository.AddAsync(submission, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(submission.Id);
        }
    }
}
