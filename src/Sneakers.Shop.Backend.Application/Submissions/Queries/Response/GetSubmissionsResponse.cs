using Sneakers.Shop.Backend.Application.Submissions.DTOs;

namespace Sneakers.Shop.Backend.Application.Submissions.Queries.Response
{
    public record GetSubmissionsResponse(
        IReadOnlyList<GetSubmissionDto> Submissions,
        int TotalCount);
}
