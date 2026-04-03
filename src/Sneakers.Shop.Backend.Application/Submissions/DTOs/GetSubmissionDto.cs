namespace Sneakers.Shop.Backend.Application.Submissions.DTOs
{
    public record GetSubmissionDto(
    Guid Id,
    string ProductName,
    string Brand,
    decimal Price,
    string Status,
    DateTimeOffset CreatedAt,
    string? RejectionReason);
}
