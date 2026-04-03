using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.DTOs;
using Sneakers.Shop.Backend.Application.Submissions.Queries.Response;
using Sneakers.Shop.Backend.Domain.Exceptions;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Application.Submissions.Queries.GetListSubmission
{
    public class GetListSubmissionQueryHandler(
        IProductSubmissionRepository productRepository) 
        : IRequestHandler<GetMySubmissionsQuery, GetSubmissionsResponse>
    {
        private readonly IProductSubmissionRepository _productRepository = productRepository;
        public async Task<GetSubmissionsResponse> Handle(
            GetMySubmissionsQuery request, 
            CancellationToken cancellationToken)
        {
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
                RejectionReason: s.RejectionReason
            )).ToList();

            return new GetSubmissionsResponse(submissionsDtos, getTotalCount);
        }
    }
}
