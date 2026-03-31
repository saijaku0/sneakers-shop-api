using FluentValidation;
using Sneakers.Shop.Backend.Application.Auth.DTOs;

namespace Sneakers.Shop.Backend.Application.Auth.Validations
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d).{6,}$")
                .WithMessage("Password must contain at least one letter and one number.");
        }
    }
}
