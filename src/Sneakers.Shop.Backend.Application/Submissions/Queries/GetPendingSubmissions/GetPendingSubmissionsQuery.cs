using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.Queries.Response;


namespace Sneakers.Shop.Backend.Application.Submissions.Queries.GetPendingSubmissions
{
    public record GetPendingSubmissionsQuery(
        Guid ModeratorId,
        int Page,
        int PageSize) : IRequest<GetPendingSubmissionsResponse>;
}
