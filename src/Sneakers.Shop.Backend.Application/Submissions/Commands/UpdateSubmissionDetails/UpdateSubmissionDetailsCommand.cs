using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.UpdateSubmissionDetails
{
    public record UpdateSubmissionDetailsCommand(
        Guid SubmissionId,
        string ProductName,
        string Description,
        string Model,
        Audience TargetAudience,
        decimal BasePrice,
        Guid BrandId) : IRequest<Result>;
}
