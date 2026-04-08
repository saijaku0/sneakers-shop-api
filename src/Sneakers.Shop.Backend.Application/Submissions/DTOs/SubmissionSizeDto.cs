using Sneakers.Shop.Backend.Domain.Enums;

namespace Sneakers.Shop.Backend.Application.Submissions.DTOs
{
    public record SubmissionSizeDto(
    int Quantity,
    decimal SizeValue,
    MeasureSizes MeasureType);
}
