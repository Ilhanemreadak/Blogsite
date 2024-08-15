using Blog.Entity.Entities;
using FluentValidation;

namespace Blog.Service.FluentValidations
{
    public class AboutValidator : AbstractValidator<About>
    {
        public AboutValidator() {
            RuleFor(c => c.Title)
                .NotEmpty()
                .NotNull()
                .MinimumLength(5)
                .MaximumLength(70);
            RuleFor(c => c.Description)
                .NotEmpty()
                .NotNull()
                .MinimumLength(20)
                .MaximumLength(500);
        }

    }
}
