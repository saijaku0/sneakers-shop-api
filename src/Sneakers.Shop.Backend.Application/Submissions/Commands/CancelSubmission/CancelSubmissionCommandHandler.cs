using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.CancelSubmission
{
    public class CancelSubmissionCommandHandler(
        IProductSubmissionRepository submissionRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CancelSubmissionCommand, Result>
    {
        private readonly IProductSubmissionRepository _submissionRepository = submissionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result> Handle(CancelSubmissionCommand request, CancellationToken cancellationToken)
        {
            var submission = await _submissionRepository
                .GetByIdAsync(request.SubmissionId, cancellationToken);
            if (submission == null) 
                return Result.Failure(Error.NotFound("Submission not found."));
            if (submission.DropId != request.DropId)
                return Result.Failure(Error.Conflict("Submission does not belong to the specified drop."));

            submission.Cancel();
            _submissionRepository.Update(submission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
