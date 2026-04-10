using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.Queries.Response;
using Sneakers.Shop.Backend.Domain.Common;


namespace Sneakers.Shop.Backend.Application.Submissions.Queries.GetPendingSubmissions
{
    public record GetPendingSubmissionsQuery(
        Guid ModeratorId,
        int Page,
        int PageSize) : IRequest<Result<GetPendingSubmissionsResponse>>;
}
