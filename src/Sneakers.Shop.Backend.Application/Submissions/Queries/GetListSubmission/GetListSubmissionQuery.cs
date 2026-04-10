using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.Queries.Response;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Submissions.Queries.GetListSubmission
{
    public record GetMySubmissionsQuery(
        Guid DropId,
        int Page,
        int PageSize) : IRequest<Result<GetSubmissionsResponse>>;
}
