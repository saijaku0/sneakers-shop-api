using MediatR;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.CancelSubmission
{
    public record CancelSubmissionCommand(
        Guid SubmissionId,
        Guid DropId) : IRequest<Unit>;
}
