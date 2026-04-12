using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Interfaces;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.ApproveSubmission
{
    public class ApproveSubmissionHandler(
        IProductSubmissionRepository productSubmission, 
        IUnitOfWork unitOfWork, 
        IStorageService storageService) 
        : IRequestHandler<ApproveSubmissionCommand, Result>
    {
        private readonly IProductSubmissionRepository _productSubmission = productSubmission;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IStorageService _storageService = storageService;
        public async Task<Result> Handle(
            ApproveSubmissionCommand request, 
            CancellationToken cancellationToken)
        {
            var submission = await _productSubmission.GetByIdAsync(request.SubmissionId, cancellationToken);
            if (submission is null)
                return Result.Failure(Error.NotFound("Submission not found"));
            if (request.ModeratorId == submission.DropId)
                return Result.Failure(Error.Unauthorized("You cannot approve your own submission"));
            var imageUrls = await _storageService.UploadManyAsync(
                request.Files.Select(f => (f.Stream, f.FileName)),
                $"submissions/{request.SubmissionId}", 
                cancellationToken);

            submission.Approve(request.ModeratorId, imageUrls);
            _productSubmission.Update(submission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
