using FluentValidation;

namespace Atlas.Forum.Application.Posts.AddPost;

internal sealed class AddPostValidator : AbstractValidator<AddPostCommand>
{
    public AddPostValidator()
    {
        RuleFor(c => c.Content).NotEmpty().MaximumLength(5000);
    }
}
