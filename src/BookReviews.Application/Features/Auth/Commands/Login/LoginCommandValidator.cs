using FluentValidation;

namespace BookReviews.Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {    
        public LoginCommandValidator()
        {
            RuleFor(p => p.Email)  
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .EmailAddress().WithMessage("A valid email is required.");
              
            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MinimumLength(6).WithMessage("{PropertyName} must be at least 6 characters.");
        }
    }
}
