using MediatR;
using Sneakers.Shop.Backend.Domain.Exceptions;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.UpdateSubmission
{
    public class UpdateSubmissionCommandHandler(
        IProductSubmissionRepository submissionRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateSubmissionCommand, Unit>
    {
        private readonly IProductSubmissionRepository _submissionRepository = submissionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(
            UpdateSubmissionCommand request, 
            CancellationToken cancellationToken)
        {
            var submission = await _submissionRepository
                .GetByIdAsync(request.SubmissionId, cancellationToken)
                ?? throw new DomainException($"Submission with ID {request.SubmissionId} not found.");

            if (submission.DropId != request.DropId)
                throw new DomainException("Submission does not belong to the specified drop.");

            submission.UpdateDetails(
                request.Payload.TargetAudience,
                request.Payload.BrandId,
                request.Payload.ProductName,
                request.Payload.Model,
                request.Payload.Description,
                request.Payload.BasePrice);

            _submissionRepository.Update(submission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
