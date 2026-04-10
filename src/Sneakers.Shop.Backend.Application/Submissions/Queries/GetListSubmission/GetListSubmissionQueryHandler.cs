using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.DTOs;
using Sneakers.Shop.Backend.Application.Submissions.Queries.Response;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Enums;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Queries.GetListSubmission
{
    public class GetListSubmissionQuery(
        IProductSubmissionRepository productRepository) 
        : IRequestHandler<GetMySubmissionsQuery, Result<GetSubmissionsResponse>>
    {
        private readonly IProductSubmissionRepository _productRepository = productRepository;
        public async Task<Result<GetSubmissionsResponse>> Handle(
            GetMySubmissionsQuery request, 
            CancellationToken cancellationToken)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                return Result<GetSubmissionsResponse>.Failure(Error.BadRequest("Page and PageSize must be greater than 0."));

            var submissions = await _productRepository.GetByDropperIdAsync(
                request.DropId, 
                request.Page, 
                request.PageSize, 
                cancellationToken);

            var getTotalCount = await _productRepository.GetTotalCountByDropperIdAsync(
                request.DropId, 
                cancellationToken);
            
            var submissionsDtos = submissions.Select(s => new GetSubmissionDto
            (
                Id: s.Id,
                ProductName: s.ProductName,
                Brand: s.SneakersBrand?.BrandName ?? "Unknown",
                Price: s.BasePrice,
                Status: s.Status.ToString(),
                CreatedAt: s.CreatedAt,
                RejectionReason: s.RejectionReason,
                SubmissionSizes: [.. s.SubmissionSizes.Select(size => new SubmissionSizeDto(
                    size.Quantity,
                    size.SizeInCm,
                    MeasureSizes.CM
                ))]
            )).ToList();

            var response = new GetSubmissionsResponse(submissionsDtos, getTotalCount);
            return Result<GetSubmissionsResponse>.Success(response);
        }
    }
}
