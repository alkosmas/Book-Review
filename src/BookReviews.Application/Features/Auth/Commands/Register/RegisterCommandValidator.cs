using FluentValidation;

namespace BookReviews.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(p => p.Username)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters");
            
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .EmailAddress().WithMessage("{PropertyName} is required");
            
            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MinimumLength(6).WithMessage("{PropertyName} must be at least 6 characters.");
        }
    }
}
