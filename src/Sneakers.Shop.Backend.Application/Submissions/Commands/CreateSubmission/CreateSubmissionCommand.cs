using MediatR;
using Sneakers.Shop.Backend.Application.Submissions.DTOs;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.CreateSubmission
{
    public record CreateSubmissionCommand(
        Guid DropId,
        Guid BrandId,
        Audience TargetAudience,
        string ProductName,
        string Model,
        string Description,
        decimal BasePrice,
        IReadOnlyCollection<SubmissionSizeDto> SubmissionSizes) : IRequest<Result<Guid>>;
}
