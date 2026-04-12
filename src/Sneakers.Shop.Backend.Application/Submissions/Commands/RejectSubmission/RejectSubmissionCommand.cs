using MediatR;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.RejectSubmission
{
    public record RejectSubmissionCommand(
        Guid SubmissionId,
        Guid ModeratorId,
        string Reason) : IRequest<Result>;
}
