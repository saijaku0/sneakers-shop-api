namespace Sneakers.Shop.Backend.Domain.Events
{
    public record SubmissionApprovedEvent(Guid SubmissionId, Guid ProductId) 
        : IDomainEvent;
}
