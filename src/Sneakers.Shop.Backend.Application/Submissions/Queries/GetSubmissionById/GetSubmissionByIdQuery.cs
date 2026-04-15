using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.DTOs;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Submissions.Queries.GetSubmissionById
{
    public record GetSubmissionByIdQuery(
        Guid? DropId,
        Guid SubmissionId) : IRequest<Result<GetSubmissionDto>>;
}
