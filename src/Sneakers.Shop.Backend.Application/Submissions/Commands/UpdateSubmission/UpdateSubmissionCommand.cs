using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.DTOs;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.UpdateSubmission
{
    public record UpdateSubmissionCommand(
        Guid DropId,
        Guid SubmissionId,
        UpdateSubmissionRequest Payload
        ) : IRequest<Result>;
}
