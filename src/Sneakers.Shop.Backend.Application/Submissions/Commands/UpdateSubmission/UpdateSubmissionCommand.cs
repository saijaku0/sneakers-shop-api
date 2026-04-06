using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.DTOs;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.UpdateSubmission
{
    public record UpdateSubmissionCommand(
        Guid DropId,
        Guid SubmissionId,
        UpdateSubmissionRequest Payload
        ) : IRequest<Unit>;
}
