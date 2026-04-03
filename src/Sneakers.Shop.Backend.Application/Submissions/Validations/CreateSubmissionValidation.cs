using FluentValidation;
using Sneakers.Shop.Backend.Application.Submissions.Commands.CreateSubmission;

namespace Sneakers.Shop.Backend.Application.Submissions.Validations
{
    public class CreateSubmissionValidation 
        : AbstractValidator<CreateSubmissionCommand>
    {
        public CreateSubmissionValidation() 
        {
            RuleFor(x => x.DropId)
                .NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("Brand ID is required.");
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model is required.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
            RuleFor(x => x.BasePrice)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Base price cannot be negative.");
            RuleFor(x => x.TargetAudience)
                .IsInEnum()
                .WithMessage("Invalid target audience.");
        }
    }
}
