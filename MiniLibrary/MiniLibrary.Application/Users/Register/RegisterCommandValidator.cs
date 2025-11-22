using FluentValidation;

namespace MiniLibrary.Application.Users.Register;

internal sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.FullName)
            .NotEmpty()
            .WithMessage("Full name is required");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long")
            .MaximumLength(100)
            .WithMessage("Password must not exceed 100 characters");

        RuleFor(c => c.Role)
            .IsInEnum()
            .WithMessage("Invalid role specified");
    }
}
