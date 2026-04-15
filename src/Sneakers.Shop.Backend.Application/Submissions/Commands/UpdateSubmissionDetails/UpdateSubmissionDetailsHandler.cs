using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.UpdateSubmissionDetails
{
    public class UpdateSubmissionDetailsHandler(
        IProductSubmissionRepository submissionRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateSubmissionDetailsCommand, Result>
    {
        private readonly IProductSubmissionRepository _submissionRepository = submissionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result> Handle(
            UpdateSubmissionDetailsCommand request,
            CancellationToken cancellationToken)
        {
            var submission = await _submissionRepository.GetByIdAsync(request.SubmissionId, cancellationToken);
            if (submission == null)
                return Result.Failure(Error.NotFound($"Submission with id {request.SubmissionId} not found."));
            submission.UpdateDetails(
                productName: request.ProductName,
                description: request.Description,
                targetAudience: request.TargetAudience,
                model: request.Model,
                basePrice: request.BasePrice,
                brandId: request.BrandId);
            _submissionRepository.Update(submission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
