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
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");
            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage("Lastname is required.")
                .MaximumLength(50).WithMessage("Lastname cannot exceed 50 characters.");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[1-9]\d{7,14}$")
                .WithMessage("Invalid phone number format. Use international format, e.g. +491234567890.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d).{6,}$")
                .WithMessage("Password must contain at least one letter and one number.");
        }
    }
}
