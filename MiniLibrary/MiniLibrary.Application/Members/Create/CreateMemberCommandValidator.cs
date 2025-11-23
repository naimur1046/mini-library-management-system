using FluentValidation;

namespace MiniLibrary.Application.Members.Create;

internal sealed class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(c => c.FullName)
            .NotEmpty()
            .WithMessage("Full name is required");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

        RuleFor(c => c.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^(?:\+8801|01)[3-9]\d{8}$")
            .WithMessage("Phone number must be a valid Bangladeshi number");

        RuleFor(c => c.JoinDate)
            .NotEmpty()
            .WithMessage("Join Date is required");

        RuleFor(c => c.IsActive)
            .NotNull()
            .WithMessage("IsActive must be specified");
    }
}
