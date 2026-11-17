using FluentValidation;

namespace Atlas.Forum.Application.Threads.CreateThread;

internal sealed class CreateThreadValidator : AbstractValidator<CreateThreadCommand>
{
    public CreateThreadValidator()
    {
        RuleFor(c => c.Title).NotEmpty().MinimumLength(3).MaximumLength(150);
        RuleFor(c => c.Category).IsInEnum();
        RuleFor(c => c.Content).NotEmpty().MaximumLength(5000);
    }
}
