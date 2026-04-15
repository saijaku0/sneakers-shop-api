using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.DTOs;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Queries.GetSubmissionById
{
    public class GetSubmissionByIdQueryHandler
        (IProductSubmissionRepository submissionRepository)
        : IRequestHandler<GetSubmissionByIdQuery, Result<GetSubmissionDto>>
    {
        private readonly IProductSubmissionRepository _submissionRepository = submissionRepository;
        public async Task<Result<GetSubmissionDto>> Handle(
            GetSubmissionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var submission = await _submissionRepository
                .GetByIdWithDetailsAsync(request.SubmissionId, cancellationToken);
            if (submission == null)
                return Result<GetSubmissionDto>.Failure(Error
                    .NotFound($"Submission with id {request.SubmissionId} not found."));
            if (request.DropId.HasValue && submission.DropId != request.DropId)
                return Result<GetSubmissionDto>.Failure(Error
                    .NotFound($"Submission with id {request.SubmissionId} not found in drop {request.DropId}."));

            var response = new GetSubmissionDto(
                Id: submission.Id,
                ProductName: submission.ProductName,
                Brand: submission.SneakersBrand?.BrandName ?? "Unknown",
                Price: submission.BasePrice,
                Status: submission.Status.ToString(),
                CreatedAt: submission.CreatedAt,
                RejectionReason: submission.RejectionReason,
                SubmissionSizes: [.. submission.SubmissionSizes.Select(size => new SubmissionSizeDto(
                    size.Quantity,
                    size.SizeInCm,
                    MeasureSizes.CM
                ))]
            );
            return Result<GetSubmissionDto>.Success(response);
        }
    }
}
