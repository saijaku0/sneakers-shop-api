using MediatR;
using Sneakers.Shop.Backend.Domain.Common;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.ApproveSubmission
{
    public record ApproveSubmissionCommand(
        Guid SubmissionId,
        Guid ModeratorId,
        IEnumerable<(Stream Stream, string FileName)> Files) : IRequest<Result>;
}
