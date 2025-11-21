using FluentValidation;

namespace MiniLibrary.Application.Users.Login;

internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}
