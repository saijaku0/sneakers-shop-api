using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Application.Submissions.DTOs
{
    public record UpdateSubmissionRequest(
        Guid BrandId,
        Audience TargetAudience,
        string ProductName,
        string Model,
        string Description,
        decimal BasePrice
        IReadOnlyCollection<SubmissionSizeDto> SubmissionSizes);
}
