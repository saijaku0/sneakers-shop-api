using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.DTOs;
using Sneakers.Shop.Backend.Domain.Exceptions;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Queries.GetSubmissionById
{
    public class GetSubmissionByIdQueryHandler
        (IProductSubmissionRepository submissionRepository)
        : IRequestHandler<GetSubmissionByIdQuery, GetSubmissionDto>
    {
        private readonly IProductSubmissionRepository _submissionRepository = submissionRepository;
        public async Task<GetSubmissionDto> Handle(
            GetSubmissionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var submission = await _submissionRepository
                .GetByIdWithDetailsAsync(request.SubmissionId, cancellationToken)
                ?? throw new DomainException($"Submission with id {request.SubmissionId} not found.");

            if (submission.DropId != request.DropId)
                throw new DomainException($"Submission with id {request.SubmissionId} not found in drop {request.DropId}.");

            return new GetSubmissionDto(
                Id: submission.Id,
                ProductName: submission.ProductName,
                Brand: submission.SneakersBrand?.BrandName ?? "Unknown",
                Price: submission.BasePrice,
                Status: submission.Status.ToString(),
                CreatedAt: submission.CreatedAt,
                RejectionReason: submission.RejectionReason
            );
        }
    }
}
