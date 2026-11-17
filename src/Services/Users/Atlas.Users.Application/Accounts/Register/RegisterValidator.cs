using FluentValidation;

namespace Atlas.Users.Application.Accounts.Register;

internal sealed class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(c => c.UserName).NotEmpty().MinimumLength(3).MaximumLength(40);
        RuleFor(c => c.Password).NotEmpty().MinimumLength(8).WithMessage("Hasło musi mieć co najmniej 8 znaków.");
    }
}
