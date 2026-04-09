namespace Sneakers.Shop.Backend.Application.Submissions.DTOs
{
    public record GetPendingSubmissionDto(
        Guid Id,
        string ProductName,
        string Brand,
        decimal Price,
        string Status,
        DateTimeOffset CreatedAt,
        string? RejectionReason,
        IReadOnlyCollection<SubmissionSizeDto> SubmissionSizes,
        Guid DropId,
        string DropperName) 
        : GetSubmissionDto(Id, ProductName, Brand, Price, Status, CreatedAt, RejectionReason, SubmissionSizes);
}
