using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;
using Sneakers.Shop.Backend.Domain.Interfaces;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.UpdateSubmission
{
    public class UpdateSubmissionCommandHandler(
        IProductSubmissionRepository submissionRepository,
        ISizeConversionService sizeConversionService,
        IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateSubmissionCommand, Result>
    {
        private readonly IProductSubmissionRepository _submissionRepository = submissionRepository;
        private readonly ISizeConversionService _sizeConversionService = sizeConversionService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(
            UpdateSubmissionCommand request, 
            CancellationToken cancellationToken)
        {
            var submission = await _submissionRepository
                .GetByIdAsync(request.SubmissionId, cancellationToken);
            if (submission == null)
                return Result.Failure(Error.NotFound($"Submission with ID {request.SubmissionId} not found."));

            if (submission.DropId != request.DropId)
                return Result.Failure(Error.Conflict("Submission does not belong to the specified drop."));

            submission.UpdateDetails(
                request.Payload.TargetAudience,
                request.Payload.BrandId,
                request.Payload.ProductName,
                request.Payload.Model,
                request.Payload.Description,
                request.Payload.BasePrice);

            submission.SetSizeList(
                request.Payload.SubmissionSizes.Select(s =>
                {
                    var sizeInCm = _sizeConversionService.GetEquivalentSize(
                        s.SizeValue,
                        s.MeasureType,
                        MeasureSizes.CM,
                        request.Payload.TargetAudience
                    );
                    return (s.Quantity, sizeInCm);
                })
            );

            _submissionRepository.Update(submission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
