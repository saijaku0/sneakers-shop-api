using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.DTOs;
using Sneakers.Shop.Backend.Application.Submissions.Queries.Response;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Exceptions;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Queries.GetPendingSubmissions
{
    public class GetPendingSubmissionsQueryHandler(
    IProductSubmissionRepository submissionRepository)
    : IRequestHandler<GetPendingSubmissionsQuery, GetPendingSubmissionsResponse>
    {
        private readonly IProductSubmissionRepository _submissionRepository = submissionRepository;

        public async Task<GetPendingSubmissionsResponse> Handle(
            GetPendingSubmissionsQuery request,
            CancellationToken cancellationToken)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                throw new DomainException("Page must be >= 1 and pageSize more than 0.");

            var submissions = await _submissionRepository.GetPendingAsync(
                request.Page,
                request.PageSize,
                cancellationToken);

            var total = await _submissionRepository.GetTotalPendingCountAsync(cancellationToken);

            var dtos = submissions.Select(s => new GetPendingSubmissionDto(
                Id: s.Id,
                ProductName: s.ProductName,
                Brand: s.SneakersBrand?.BrandName ?? "Unknown",
                Price: s.BasePrice,
                Status: s.Status.ToString(),
                CreatedAt: s.CreatedAt,
                RejectionReason: s.RejectionReason,
                SubmissionSizes: s.SubmissionSizes.Select(sz => new SubmissionSizeDto(
                    sz.Quantity,
                    sz.SizeInCm,
                    MeasureSizes.CM
                )).ToList(),
                DropId: s.DropId,
                DropperName: s.Dropper != null ? $"{s.Dropper.Name} {s.Dropper.Lastname}" : "Unknown"
            )).ToList();

            return new GetPendingSubmissionsResponse(dtos, total);
        }
    }
}
