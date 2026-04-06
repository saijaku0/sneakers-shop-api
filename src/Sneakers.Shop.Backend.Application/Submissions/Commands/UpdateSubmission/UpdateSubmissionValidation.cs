using FluentValidation;

namespace Sneakers.Shop.Backend.Application.Submissions.Commands.UpdateSubmission
{
    public class UpdateSubmissionValidation : AbstractValidator<UpdateSubmissionCommand>
    {
        public UpdateSubmissionValidation() 
        {
            RuleFor(x => x.DropId)
                .NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.Payload.BrandId)
                .NotEmpty().WithMessage("Brand ID is required.");
            RuleFor(x => x.Payload.ProductName)
                .NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Payload.Model)
                .NotEmpty().WithMessage("Model is required.");
            RuleFor(x => x.Payload.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
            RuleFor(x => x.Payload.BasePrice)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Base price cannot be negative.");
            RuleFor(x => x.Payload.TargetAudience)
                .IsInEnum()
                .WithMessage("Invalid target audience.");
        }
    }
}
