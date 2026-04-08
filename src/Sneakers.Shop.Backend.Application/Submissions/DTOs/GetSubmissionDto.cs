namespace Sneakers.Shop.Backend.Application.Submissions.DTOs
{
    /// <summary>
    /// Represents the data transfer object for retrieving information about a product submission, including its status,
    /// pricing, and related metadata.
    /// </summary>
    /// <param name="Id">The unique identifier of the submission.</param>
    /// <param name="ProductName">The name of the submitted product.</param>
    /// <param name="Brand">The brand associated with the product.</param>
    /// <param name="Price">The price of the product at the time of submission.</param>
    /// <param name="Status">The current status of the submission, such as pending, approved, or rejected.</param>
    /// <param name="CreatedAt">The date and time when the submission was created, expressed as a UTC offset.</param>
    /// <param name="RejectionReason">The reason for rejection if the submission was not approved; otherwise, null.</param>
    public record GetSubmissionDto(
    Guid Id,
    string ProductName,
    string Brand,
    decimal Price,
    string Status,
    DateTimeOffset CreatedAt,
    string? RejectionReason,
    IReadOnlyCollection<SubmissionSizeDto> SubmissionSizes);
}
