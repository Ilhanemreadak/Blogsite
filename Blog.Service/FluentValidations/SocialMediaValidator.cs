using Blog.Entity.Entities;
using FluentValidation;

namespace Blog.Service.FluentValidations
{
    public class SocialMediaValidator : AbstractValidator<SocialMedia>
    {
        public SocialMediaValidator() {
            RuleFor(c => c.Link)
                .NotEmpty()
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(240);
        }
    }
}
