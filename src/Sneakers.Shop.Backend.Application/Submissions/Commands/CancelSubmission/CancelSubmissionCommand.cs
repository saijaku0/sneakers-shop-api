using MediatR;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.CancelSubmission
{
    public record CancelSubmissionCommand(
        Guid SubmissionId,
        Guid DropId) : IRequest<Result>;
}
