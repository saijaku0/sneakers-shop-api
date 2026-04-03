using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.Queries.Response;

namespace Sneakers.Shop.Backend.Application.Submissions.Queries.GetListSubmission
{
    public record GetMySubmissionsQuery(
        Guid DropId,
        int Page,
        int PageSize) : IRequest<GetSubmissionsResponse>;
}
