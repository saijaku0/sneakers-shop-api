using MediatR;
using Sneakers.Shop.Backend.Domain.Exceptions;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.CancelSubmission
{
    public class CancelSubmissionCommandHandler(
        IProductSubmissionRepository submissionRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CancelSubmissionCommand, Unit>
    {
        private readonly IProductSubmissionRepository _submissionRepository = submissionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(CancelSubmissionCommand request, CancellationToken cancellationToken)
        {
            var submission = await _submissionRepository
                .GetByIdAsync(request.SubmissionId, cancellationToken)
                ?? throw new DomainException("Submission not found.");
            if (submission.DropId != request.DropId)
                throw new DomainException("Submission does not belong to the specified drop.");
            submission.Cancel();
            _submissionRepository.Update(submission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
