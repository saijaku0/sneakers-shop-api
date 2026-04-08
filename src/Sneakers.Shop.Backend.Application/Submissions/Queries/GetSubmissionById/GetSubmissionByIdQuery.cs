using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.DTOs;

namespace Sneakers.Shop.Backend.Application.Submissions.Queries.GetSubmissionById
{
    public record GetSubmissionByIdQuery(
        Guid DropId,
        Guid SubmissionId) : IRequest<GetSubmissionDto>;
}
