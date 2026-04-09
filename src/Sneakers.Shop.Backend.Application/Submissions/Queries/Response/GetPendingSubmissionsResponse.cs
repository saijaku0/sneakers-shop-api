using Sneakers.Shop.Backend.Application.Submissions.DTOs;

namespace Sneakers.Shop.Backend.Application.Submissions.Queries.Response
{
    public record GetPendingSubmissionsResponse(
        IReadOnlyCollection<GetPendingSubmissionDto> Submissions,
        int TotalCount);
}
