using MediatR;
using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands
{
    public record CreateSubmissionCommand(
        Guid DropId,
        Guid BrandId,
        Audience TargetAudience,
        string ProductName,
        string Model,
        string Description,
        decimal BasePrice) : IRequest<Guid>;
}
