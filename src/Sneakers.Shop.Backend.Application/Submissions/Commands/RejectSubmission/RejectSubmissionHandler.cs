using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.RejectSubmission
{
    public class RejectSubmissionHandler(
        IProductSubmissionRepository productSubmissionRepository,
        IUnitOfWork unitOfWork) 
        : IRequestHandler<RejectSubmissionCommand, Result>
    {
        private readonly IProductSubmissionRepository _productSubmissionRepository = productSubmissionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Result> Handle(
            RejectSubmissionCommand request, 
            CancellationToken cancellationToken)
        {
            var submission = await _productSubmissionRepository.GetByIdAsync(request.SubmissionId, cancellationToken);
            if (submission is null)
                return Result.Failure(Error.NotFound("Submission not found."));
            if (request.ModeratorId == submission.Dropper?.Id)
                return Result.Failure(Error.Forbidden("You cannot moderate your own submission."));
            submission.Reject(request.ModeratorId, request.Reason);
            _productSubmissionRepository.Update(submission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
